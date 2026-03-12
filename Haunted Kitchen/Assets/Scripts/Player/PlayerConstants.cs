using System.Dynamic;
using UnityEngine;

public static class PlayerConstants
{
    // Input Actions
    public const string INPUTACTION_POSSESSION = "Possession";
    public const string INPUTACTION_PLAYER = "Player";
    public const string INPUTACTION_UI = "UI";

    // Movement
    public const float MOVE_SPEED = 7f;
    public const float RUN_SPEED = 14f;
    public const float ROTATION_SPEED = 10f;

    // Physics
    public const float GRAVITY = -20f;
    public const float GROUNDED_FORCE = -2f;

    // Animations
    public const float RUN_ANIM_MULTIPLIER = 2f;
    public const string ANIM_STATE = "State";

    public const string ANIM_IDLE_ONE_HAND = "IdleOneHand";
    public const string ANIM_IDLE_TWO_HAND = "IdleTwoHand";

    public const string ANIM_SLIP = "Slip";
    public const string ANIM_ISPOSSESSED = "IsPossessed";
    public const string ANIM_STRUGGLE = "Struggle";

    public const string ANIM_DIE = "Die";

    // Interaction
    public const float HOLD_THRESHOLD = 0.4f;

    // Speed Buff
    public const float SPEED_BUFF_MULTIPLIER = 2f;
}
