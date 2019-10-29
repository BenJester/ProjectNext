using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariable 
{
    public static UIPlayerController s_UIPlayerCtrl;

    private static PlayerControl1 s_PlayerIns;

    public static UIPlayerController GetUIPlayerCtrl()
    {
        if( s_UIPlayerCtrl == null )
        {
            s_UIPlayerCtrl = GameObject.FindObjectOfType<UIPlayerController>();
        }
        return s_UIPlayerCtrl;
    }

    public static void SetPlayer(PlayerControl1 _player)
    {
        s_PlayerIns = _player;
    }

    public static PlayerControl1 GetPlayer()
    {
        return s_PlayerIns;
    }
}
