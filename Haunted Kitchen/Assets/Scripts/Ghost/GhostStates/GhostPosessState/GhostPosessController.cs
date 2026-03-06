using UnityEngine;

public class GhostPosessController : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
    }

    public Transform GetPlayer()
    {
        if (player == null) return null;
        return player.transform;
    }
}
