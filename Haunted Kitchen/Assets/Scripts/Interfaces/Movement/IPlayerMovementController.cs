using UnityEngine;

public interface IPlayerMovementController
{
    void Move(Vector3 direction, float speed);
    void Stop();
    void SetCanMove(bool canMove);
}
