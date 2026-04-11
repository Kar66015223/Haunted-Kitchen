using UnityEngine;

public class WorkerAnimation : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private GameObject mopPrefab;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void SetMove(int value)
    {
        anim.SetInteger(WorkerConstants.ANIM_MOVE, value);
    }

    public void SetOneHand(bool value)
    {
        anim.SetBool(WorkerConstants.ANIM_ONEHAND, value);
    }

    public void SetTwoHand(bool value)
    {
        anim.SetBool(WorkerConstants.ANIM_TWOHAND, value);
    }

    public void SetClean(bool value)
    {
        anim.SetBool(WorkerConstants.ANIM_CLEAN, value);
        mopPrefab.SetActive(value);
    }
}
