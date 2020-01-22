using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationComponent : MonoBehaviour
{
    public string DashChargingParam;
    public string DashProcessParam;
    public string DashEndParam;
    public string DashToIdleParam;


    public PlayerStateManager StateMgr;
    public PlayerControl1 playerControl;


    private Animator m_animator;
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        if(m_animator == null)
        {
            Debug.Assert(false);
        }
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
        if (playerControl.isTouchingGround)
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
        if(playerControl.isTouchingGround)
        {
            m_animator.SetBool(DashEndParam, false);
            m_animator.SetBool(DashProcessParam, false);
            m_animator.SetBool(DashChargingParam, false);
            m_animator.SetBool(DashToIdleParam, false);
            StateMgr.SetPlayerState(PlayerStateDefine.PlayerState_Typ.playerState_Idle);
        }
        else
        {
            //Debug.Assert(false);
            m_animator.SetBool(DashEndParam, false);
            m_animator.SetBool(DashProcessParam, false);
            m_animator.SetBool(DashChargingParam, false);
            m_animator.SetBool(DashToIdleParam, false);
            StateMgr.SetPlayerState(PlayerStateDefine.PlayerState_Typ.playerState_Jumping);
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
