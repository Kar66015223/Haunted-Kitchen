using UnityEngine;

public class GhostAnimation : MonoBehaviour, IAnimationController
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponentInParent<Animator>();
    }

    public void SetBool(string parameter, bool value)
    {
        if (anim != null)
            anim.SetBool(parameter, value);
    }

    public void SetTrigger(string parameter)
    {
        if (anim != null)
            anim.SetTrigger(parameter);
    }
}
