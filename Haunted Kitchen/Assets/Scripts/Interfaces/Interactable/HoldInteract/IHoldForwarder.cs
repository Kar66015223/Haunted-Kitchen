using UnityEngine;

public interface IHoldForwarder
{
    bool HasHoldable();
    void ForwardHold(GameObject interactor);
}
