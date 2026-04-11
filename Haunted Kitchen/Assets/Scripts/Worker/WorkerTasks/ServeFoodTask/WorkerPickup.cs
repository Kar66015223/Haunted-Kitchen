using UnityEngine;

public class WorkerPickup : MonoBehaviour
{
    public Item currentItem;
    [SerializeField] private Transform holdPoint;

    private WorkerAnimation anim;
    private Worker worker;

    void Awake()
    {
        anim = GetComponent<WorkerAnimation>();
        worker = GetComponent<Worker>();
    }

    void Update()
    {
        if(worker.GetCurrentState() == WorkerState.Idle &&
        currentItem != null)
        {
            Serve(currentItem);
        }
    }

    public void PickUp(Item item)
    {
        currentItem = item;
        item.SetWorkerHeld();

        if (item.gameObject.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }

        var colliders = item.gameObject.GetComponentsInChildren<Collider>();
        foreach (var c in colliders)
        {
            c.enabled = false;
        }

        item.gameObject.transform.SetParent(holdPoint);
        item.gameObject.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        item.SetState(ItemState.Held);

        GameEvents.OnItemPickedUp?.Invoke(currentItem);

        if (currentItem.itemData.oneHand)
        {
            anim.SetOneHand(true);
        }
        else
        {
            anim.SetTwoHand(true);
        }
    }
    
    public void Serve(Item item)
    {
        Destroy(item.gameObject);
        currentItem = null;

        anim.SetOneHand(false);
        anim.SetTwoHand(false);
    }
}
