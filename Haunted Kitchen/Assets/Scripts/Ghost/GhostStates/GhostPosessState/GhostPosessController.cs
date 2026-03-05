using UnityEngine;

public class GhostPosessController : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
    }

    public void LookAtPlayer()
    {
        if (player == null) return;

        
    }
}
