using System;
using UnityEngine;

public static class GameEvents
{
    public static Action<float> OnSpeedBuff;
    public static Action<string, Color> OnShowEventText;
    public static Action<bool> OnLightOut;
    public static Action<bool> OnUIShow;
    public static Action<bool> OnInteractPressed;
    public static Action<bool> OnToggleGhostSpawning;
    public static Action OnAddMoneyButtonClicked;
    public static Action<Item> OnItemPickedUp;
}
