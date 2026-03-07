using UnityEngine;

public class GhostAnimation : MonoBehaviour, IAnimationController
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void SetBool(string parameter, bool value)
    {
        if (anim == null)
        {
            Debug.Log("anim is null");
            return;
        }

        anim.SetBool(parameter, value);
    }

    public void SetTrigger(string parameter)
    {
        if (anim == null)
        {
            Debug.Log("anim is null");
            return;
        }

        anim.SetTrigger(parameter);
    }
}
