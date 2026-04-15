using UnityEngine;

public class DeepfryerVisual : MonoBehaviour
{
    [SerializeField] private GameObject[] oilVisual;
    private Kitchenware kitchenware;

    void Awake()
    {
        kitchenware = GetComponent<Kitchenware>();
    }

    void Update()
    {
        if(oilVisual == null)
        {
            Debug.LogError("oilVisual not assigned");
            return;
        }

        foreach (var visual in oilVisual)
        {
            visual.SetActive(kitchenware.IsCooking);
        }

        if (kitchenware.CurrentItem != null)
        {
            if (kitchenware.IsCooking)
            {
                kitchenware.ToggleItemVisual(false);
            }
            else
            {
                kitchenware.ToggleItemVisual(true);
                kitchenware.ClearItemVisual();
            }
        }
    }
}
