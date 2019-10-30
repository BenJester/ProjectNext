using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalVariable 
{
    public static UIPlayerController s_UIPlayerCtrl;

    private static PlayerControl1 s_PlayerIns;

    private static UnityAction<PlayerControl1> m_playerAc;

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
        if(m_playerAc != null)
        {
            m_playerAc.Invoke(_player);
        }
    }

    public static PlayerControl1 GetPlayer()
    {
        return s_PlayerIns;
    }

    public static void RegisteSetPlayerEvent(UnityAction<PlayerControl1> _ac)
    {
        m_playerAc += _ac;
    }
    public static void UnregisteSetPlayerEvent(UnityAction<PlayerControl1> _ac)
    {
        m_playerAc -= _ac;
    }
}
