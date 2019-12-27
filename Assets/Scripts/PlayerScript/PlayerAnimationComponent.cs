﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationComponent : MonoBehaviour
{
    public string DashChargingParam;
    public string DashProcessParam;
    public string DashEndParam;
    public string DashToIdleParam;


    private Animator m_animator;
    private PlayerControl1 m_playerControl;
    private PlayerStateManager m_stateMgr;
    // Start is called before the first frame update
    void Start()
    {
        m_stateMgr = GetComponent<PlayerStateManager>();
        m_animator = GetComponentInChildren<Animator>();
        if(m_animator == null)
        {
            Debug.Assert(false);
        }
        m_playerControl = GetComponent<PlayerControl1>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerDashCharging()
    {
        m_animator.SetBool(DashChargingParam, true);
        m_animator.SetBool(DashProcessParam, false);
        m_animator.SetBool(DashEndParam, false);
    }
    public void PlayerDashStart()
    {
        //Debug.Assert(false);
        m_animator.SetBool(DashChargingParam, false);
        m_animator.SetBool(DashProcessParam, true);
        m_animator.SetBool(DashEndParam, false);
    }
    public void PlayerDashStop()
    {
        if (m_playerControl.isTouchingGround)
        {
            m_animator.SetBool(DashChargingParam, false);
            m_animator.SetBool(DashEndParam, true);
            m_animator.SetBool(DashProcessParam, false);
            m_animator.SetBool(DashToIdleParam, true);
        }
        else
        {
            m_animator.SetBool(DashChargingParam, false);
            m_animator.SetBool(DashEndParam, true);
            m_animator.SetBool(DashProcessParam, false);
            m_animator.SetBool(DashToIdleParam, false);
        }
    }
    public void PlayerDashToIdle()
    {
        if(m_playerControl.isTouchingGround)
        {
            m_animator.SetBool(DashEndParam, false);
            m_animator.SetBool(DashProcessParam, false);
            m_animator.SetBool(DashChargingParam, false);
            m_animator.SetBool(DashToIdleParam, false);
            m_stateMgr.SetPlayerState(PlayerStateDefine.PlayerState_Typ.playerState_Idle);
        }
        else
        {
            //Debug.Assert(false);
            m_animator.SetBool(DashEndParam, false);
            m_animator.SetBool(DashProcessParam, false);
            m_animator.SetBool(DashChargingParam, false);
            m_animator.SetBool(DashToIdleParam, false);
            m_stateMgr.SetPlayerState(PlayerStateDefine.PlayerState_Typ.playerState_Jumping);
        }
    }
    public void PlayerToIdle()
    {
        m_animator.SetBool(DashEndParam, false);
        m_animator.SetBool(DashProcessParam, false);
        m_animator.SetBool(DashChargingParam, false);
        m_animator.SetBool(DashToIdleParam, false);
    }
}
