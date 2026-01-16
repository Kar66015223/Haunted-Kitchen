using UnityEngine;

public class InteractTest : MonoBehaviour, Iinteractable
{
    public void Interact(GameObject interactor)
    {
        Debug.Log($"{gameObject.name} interacted with by {interactor.name}");
    }
}
