using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Jamjardavies.Zenject.ViewController
{
    public class SceneManager : Controller, ITickable
    {
        [Inject]
        private TransientFactory m_factory = null;

        [Inject]
        private Canvas m_canvas = null;

        [InjectOptional]
        private TransitionManager m_transitionManager = null;

        private IController m_controller;
        private ITickable m_tickController;

        private Type m_type;
        private object[] m_args;

        private Stack<SceneInfo> m_sceneStack = new Stack<SceneInfo>();
        private SceneInfo m_currentScene;

        public override void Initialise()
        {
            if (m_transitionManager != null)
            {
                m_transitionManager.TransitionInFinished += OnTransitionInFinished;
            }
        }

        public override void OnDestroy()
        {
            if (m_transitionManager != null)
            {
                m_transitionManager.TransitionInFinished -= OnTransitionInFinished;
            }

            DestroyScene();
        }

        public void Tick()
        {
            if (m_tickController != null)
            {
                m_tickController.Tick();
            }
        }

        public void ChangeScene(Type type, TransitionMode useTransiiton, object[] args)
        {
            ChangeScene(type, useTransiiton, true, args);
        }

        public void GoBack()
        {
            SceneInfo info = m_sceneStack.Pop();

            m_currentScene = info;

            ChangeScene(info.SceneType, info.TransitionMode, false, info.Args);
        }

        public void ClearNavigationHistory()
        {
            m_sceneStack.Clear();
        }

        private void ChangeScene(Type type, TransitionMode useTransiiton, bool pushToStack, object[] args)
        {
            m_type = type;
            m_args = args;

            if (pushToStack)
            {
                // Push the scene we're changing from into the history.
                m_sceneStack.Push(m_currentScene);

                // Track the new scene.
                m_currentScene = new SceneInfo
                {
                    SceneType = type,
                    TransitionMode = useTransiiton,
                    Args = args
                };
            }
            
            if (m_transitionManager == null || useTransiiton == TransitionMode.Off)
            {
                OnTransitionInFinished();
            }
            else
            {
                m_transitionManager.StartTransition();
            }
        }

        private IController CreateController()
        {
            IController nextController = m_factory.Create(m_type, m_args);
            IViewController viewController = nextController as IViewController;

            if (viewController != null)
            {
                viewController.View.GameObject.transform.SetParent(m_canvas.transform, false);
            }

            // Remember to initialize it.
            nextController.Initialize();

            return nextController;
        }

        private void DestroyScene()
        {
            if (m_controller != null)
            {
                m_controller.Dispose();
                m_controller = null;
            }

            m_tickController = null;
        }

        private void OnTransitionInFinished()
        {
            IController nextController = CreateController();

            DestroyScene();

            // Unset the values, just incase ;)
            m_type = null;
            m_args = null;

            m_controller = nextController;

            // Assign the tick controller if it's an ITickable.
            m_tickController = nextController as ITickable;
        }

        private struct SceneInfo
        {
            public Type SceneType { get; set; }
            public TransitionMode TransitionMode { get; set; }
            public object[] Args { get; set; }
        }
    }
}