using Unity.VisualScripting;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    public void SetSpeed(float value)
    {
        speed = value;
    }

    public void MoveToward(Vector3 target)
    {
        Vector3 direction = (target-transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    public void Stop()
    {
        speed = 0f;
    }
}
