using UnityEngine;

public class Oil : MonoBehaviour
{
    public float slipDuration = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController controller = other.GetComponent<PlayerController>();

            if (controller != null)
            {
                controller.Slip(slipDuration);
            }

            Debug.Log($"{other.gameObject.name} stepped on an oil");
            Destroy(gameObject);
        }
    }
}
