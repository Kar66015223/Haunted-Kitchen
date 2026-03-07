using UnityEngine;

public interface IAnimationController
{
    void SetBool(string parameter, bool value);
    void SetTrigger(string parameter);
    void SetInteger(string parameter, int value);
}
