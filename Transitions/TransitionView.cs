using UnityEngine;
using Jamjardavies.Zenject.ViewController;

public sealed class TransitionView : View
{
    [SerializeField]
    private Animator m_animator = null;

    public Animator Animator
    {
        get { return m_animator; }
    }
}
