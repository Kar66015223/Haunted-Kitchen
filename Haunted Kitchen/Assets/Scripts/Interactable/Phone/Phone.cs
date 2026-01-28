using UnityEngine;

public class Phone : MonoBehaviour, Iinteractable
{
    [SerializeField] private ShopUI shopUI;

    public void Interact(GameObject interactor)
    {
        shopUI.Open();
    }
}
