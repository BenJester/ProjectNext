using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateDefine 
{
    public enum PlayerState_Typ
    {
        PlayerState_None = 0,
        playerState_ChangingSpeed,
        playerState_Dash,
        playerState_Idle,
        playerState_Jumping,
        playerState_IdleDash,
    }
}
