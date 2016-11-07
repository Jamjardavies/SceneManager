using UnityEngine;
using System;

public class SceneTransitionEvents : StateMachineBehaviour
{
    private Action m_sceneTransitionInComplete = delegate { };
    private Action m_sceneTransitionOutComplete = delegate { };

    public event Action SceneTransitionInComplete
    {
        add { m_sceneTransitionInComplete += value; }
        remove { m_sceneTransitionInComplete -= value; }
    }

    public event Action SceneTransitionOutComplete
    {
        add { m_sceneTransitionOutComplete += value; }
        remove { m_sceneTransitionOutComplete -= value; }
    }
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.shortNameHash.Equals(TransitionManager.TransitionInTrigger))
        {
            m_sceneTransitionInComplete.Invoke();
        }

        if (stateInfo.shortNameHash.Equals(TransitionManager.TransitionOutTrigger))
        {
            m_sceneTransitionOutComplete.Invoke();
        }
    }
}
