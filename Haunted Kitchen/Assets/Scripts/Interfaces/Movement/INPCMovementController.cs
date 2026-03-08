using UnityEngine;

public interface INPCMovementController
{
    void SetSpeed(float speed);
    void MoveToward(Vector3 target);
    void Stop();
}
