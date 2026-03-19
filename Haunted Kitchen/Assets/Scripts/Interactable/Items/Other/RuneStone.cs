using UnityEngine;

public class RuneStone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out GhostController ghost))
        {
            ghost.HitRune();
            Destroy(gameObject);
        }
    }
}
