using System;
using Jamjardavies.Zenject.ViewController;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class TransitionManager : Controller<TransitionView>
{
    public static int TransitionInTrigger = Animator.StringToHash("TransitionIn");
    public static int TransitionOutTrigger = Animator.StringToHash("TransitionOut");
    
    private Action m_transitionInFinished = delegate { };

    public event Action TransitionInFinished
    {
        add { m_transitionInFinished += value; }
        remove { m_transitionInFinished -= value; }
    }

    [Inject]
    public TransitionManager()
    {

    }

    public override void Initialise()
    {
        SceneTransitionEvents transition = View.Animator.GetBehaviour<SceneTransitionEvents>();

        transition.SceneTransitionInComplete += TransitionInComplete;
        transition.SceneTransitionOutComplete += TransitionOutComplete;
    }

    public override void OnDestroy() { }
    
    public void StartTransition()
    {
        View.Animator.SetTrigger(TransitionInTrigger);
    }

    private void TransitionInComplete()
    {
        m_transitionInFinished.Invoke();

        View.Animator.SetTrigger(TransitionOutTrigger);
    }

    private void TransitionOutComplete()
    {
        
    }
}