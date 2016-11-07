using System;
using Zenject;

namespace Jamjardavies.Zenject.ViewController
{
    public interface ISceneController : IController
    {
    }

    public abstract class SceneController<T> : Controller<T>, ISceneController
        where T : View
    {
        [Inject]
        private SceneChangeCommand m_sceneChange = null;

        public SceneChangeCommand SceneChange
        {
            get { return m_sceneChange; }
        }

    }

    /// <summary>
    /// A command to change the scene.
    /// Type        = The controller's type to change to.
    /// Bool        = Should use transition.
    /// Object[]    = Any parameters to inject into the controller.
    /// </summary>
    public class SceneChangeCommand : Command<Type, TransitionMode, object[]>
    {
        public void Execute<T>(params object[] args)
            where T : ISceneController
        {
            Execute<T>(TransitionMode.Off, args);
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        /// <typeparam name="T">The controller to switch to.</typeparam>
        /// <param name="useTransition">Should we use a transition?</param>
        /// <param name="args">Any arguments to pass into the controller.</param>
        public void Execute<T>(TransitionMode useTransition, params object[] args)
            where T : ISceneController
        {
            Execute(typeof(T), useTransition, args);
        }
    }

    public class SceneBackCommand : Command
    {

    }

    public enum TransitionMode
    {
        Off,
        On
    }
}