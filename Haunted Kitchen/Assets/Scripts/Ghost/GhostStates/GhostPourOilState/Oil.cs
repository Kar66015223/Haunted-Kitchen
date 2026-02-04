using UnityEngine;

public class Oil : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{other.gameObject.name} step on an oil");
            Destroy(gameObject);
        }
    }
}
