using UnityEngine;

public interface IMovementController
{
    void SetSpeed(float speed);
    void MoveToward(Vector3 target);
    void Stop();
}
