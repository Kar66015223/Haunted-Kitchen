using System;
using UnityEngine;

public static class GameEvents
{
    public static Action<float> OnSpeedBuff;
    public static Action<string, Color> OnShowEventText;
}
