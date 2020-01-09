using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Rewired;

public class Dash : Skill {
	public bool isShadowDash = false;
	public float DashSpeed;
    public float CoyoteDuration;
    public float pauseDuration;
	public float reducedTimeScale;
	public int waitTime;
	public int currWaitTime;
    public int remainBulletTimeThreshold;
    public float remainBulletTimeDuration;
	public int maxCharge;
	public int charge;
    public float multiplier;

    public GameObject dashParticle;
    public GameObject dashPointer;
    public bool DashPointerShow;
    public GameObject dashChargingParticle;
    private GameObject _dashChargeParticle;

    public bool isDashing = false;
    public float disableMovementTime;

    public float DashTime;
    public float TrajectoryTimeStep;
    public int TrajectoryStepCounts;

    LineRenderer lr;
    Animator amin;

    public Vector2 dir;
    public Vector2 dashDir;




    //Rewired-------------------------------------------------
    [Header("手柄震动,马达*2")]
 
    public float motor1Level;
    public float motor2Level;
    public float motor1duration;
    public float motor2duration;

    public float DashLimitChargingTime;
    private float m_fDashLimitChargingTime;
    private Player rPlayer;

    private PlayerStateManager m_stateMgr;

    private UnityAction m_uaDashStart;
    private UnityAction m_uaDashOver;

    private PlayerAnimationComponent m_aniCom;

    private BulletTime m_bulletTime;

    private bool m_bDashCharging;
    private bool m_bChargeZero;
    private bool m_bDashStart;
    public override void Init () {

        LevelSetting _levelSet = FindObjectOfType<LevelSetting>();
        if(_levelSet != null)
        {
            active = _levelSet.DashOpen;
        }
        else
        {
            active = true;
        }
        m_bulletTime = GetComponent<BulletTime>();
        lr = GetComponent<LineRenderer> ();
        amin = GetComponent<Animator>();
        m_stateMgr = GetComponent<PlayerStateManager>();
        charge = maxCharge;

        //Rewired------------------------------------------------------------
        rPlayer = ReInput.players.GetPlayer(0);
        m_aniCom = GetComponent<PlayerAnimationComponent>();

    }

    public override bool Check () {
		return charge > 0;
	}

	void Update () {
        if(active == false)
        {
            return;
        }

        if (Input.GetMouseButton(1) || (rPlayer != null && rPlayer.GetButton("Dash")) || (playerControl.controlState == PlayerControl1.ControlWay.isMobile && TouchControl.Instance.dashDrag))
        {
            if (m_stateMgr.GetPlayerState() == PlayerStateDefine.PlayerState_Typ.playerState_Idle || m_stateMgr.GetPlayerState() == PlayerStateDefine.PlayerState_Typ.playerState_Jumping)
            {
                if (currWaitTime == 0)
                {
                    m_aniCom.PlayerDashCharging();
                    if(m_stateMgr.GetPlayerState() == PlayerStateDefine.PlayerState_Typ.playerState_Jumping)
                    {
                        m_stateMgr.SetPlayerState(PlayerStateDefine.PlayerState_Typ.playerState_Dash);
                    }
                    else if(m_stateMgr.GetPlayerState() == PlayerStateDefine.PlayerState_Typ.playerState_Idle)
                    {
                        m_stateMgr.SetPlayerState(PlayerStateDefine.PlayerState_Typ.playerState_IdleDash);
                    }
                }
                else
                {
                    int a = 0;
                }
            }
            else
            {
                if( m_stateMgr.GetPlayerState() == PlayerStateDefine.PlayerState_Typ.playerState_Dash || m_stateMgr.GetPlayerState() == PlayerStateDefine.PlayerState_Typ.playerState_IdleDash)
                {
                    currWaitTime += 1;
                }
            }
        }
        if (Input.GetMouseButtonDown(1) || rPlayer.GetButtonDown("Dash"))
            playerControl.swap.curr = 0f;

        if ( Mathf.Abs(rPlayer.GetAxis("MoveHorizontal")) > 0 )
        {
            int a = 0;
        }
        //Debug.Log(Time.timeScale);
        //播放冲刺情况下的动画
        if (isDashing)
        {
            //冲刺中动画
            //gaidao要求去除这个旋转
            //transform.rotation = Quaternion.Euler(0, 0, -AngleBetween(Vector2.up, GetComponent<Rigidbody2D>().velocity));
        }
        bool bChargingDash = false;
        //Rewired------------------------------------------------------------
        if ((Input.GetMouseButton (1) && charge >= 1 && currWaitTime >= waitTime) 
            || (rPlayer.GetButton("Dash") && charge >= 1 && currWaitTime >= waitTime)
            || (!(playerControl.controlState == PlayerControl1.ControlWay.isKeyboard) && TouchControl.Instance.dashDrag && charge >= 1 && currWaitTime >= waitTime)) {


            //蓄力粒子
            if (_dashChargeParticle == null)
            {
                _dashChargeParticle = Instantiate(dashChargingParticle, transform.position, Quaternion.identity, transform);
            }


            //显示箭头
            if (dashPointer != null)
            {
                if(DashPointerShow == true)
                {
                    Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 dir = (mouseWorldPos - (Vector2)player.transform.position).normalized;
                    dashPointer.SetActive(true);
                    dashPointer.transform.position = (Vector2)transform.position + dir * 70f;
                    dashPointer.transform.localRotation = Quaternion.Euler(0, 0, -AngleBetween(Vector2.up, dir));
                }
            }
 
            playerControl.swap.realWaitTime = playerControl.swap.waitTime;
			playerControl.targetDeltaTime = Time.fixedDeltaTime;
			playerControl.targetTimeScale = Time.timeScale;

            m_bulletTime.ActiveBulletTime(true, BulletTime.BulletTimePriority.BulletTimePriority_High);

            //辅助线指示
            DrawTrajectory();

            //播放动态残影
            playerControl.SetColShadow();

            if(isDashing == false)
            {
                if (m_bDashCharging == false)
                {
                    m_bDashCharging = true;
                    m_fDashLimitChargingTime = 0.0f;
                }
                else
                {
                    m_fDashLimitChargingTime += (Time.deltaTime / Time.timeScale);
                    if (m_fDashLimitChargingTime >= DashLimitChargingTime)
                    {
                        m_bDashCharging = false;
                        m_bulletTime.ActiveBulletTime(false, BulletTime.BulletTimePriority.BulletTimePriority_High);
                        m_bChargeZero = true;
                        playerControl.DashRequestByPlayer();
                        bChargingDash = true;
                    }
                }
            }
		}

        //Rewired------------------------------------------------------------
        if (Input.GetMouseButtonUp (1) || rPlayer.GetButtonUp("Dash"))
        {
            m_bulletTime.ActiveBulletTime(false, BulletTime.BulletTimePriority.BulletTimePriority_High);
        }
		if (playerControl.isTouchingGround && bChargingDash == false && isDashing == false)
        {
            charge = maxCharge;
		}
	}

    public void PlayerTouchGround()
    {
        if(m_bDashCharging == true)
        {
            m_bDashCharging = false;
        }
    }

    public void RequestDash()
    {
        Do();
        if (currWaitTime < remainBulletTimeThreshold)
        {
            Time.timeScale = 1f;
            playerControl.targetTimeScale = 1f;
            Time.fixedDeltaTime = playerControl.startDeltaTime;
            playerControl.targetDeltaTime = Time.fixedDeltaTime;
        }
        else
        {
            Time.timeScale = 0.1f;
            playerControl.targetTimeScale = 0.1f;
            Time.fixedDeltaTime = playerControl.startDeltaTime * 0.1f;
            playerControl.targetDeltaTime = playerControl.startDeltaTime * 0.1f;
            StartCoroutine(CancelBulletTime());
        }
        currWaitTime = 0;

        //辅助线取消
        lr.enabled = false;
        //蓄力冲刺特效取消[是不是不生成比较有效率？]
        if (_dashChargeParticle != null) Destroy(_dashChargeParticle);
        if (dashPointer != null) dashPointer.SetActive(false);
    }

    IEnumerator CancelBulletTime()
    {
        m_bulletTime.ActiveBulletTime(true, BulletTime.BulletTimePriority.BulletTimePriority_High);
        yield return new WaitForSecondsRealtime(remainBulletTimeDuration);
        playerControl.targetTimeScale = 1f;
        playerControl.targetDeltaTime = playerControl.startDeltaTime * 1f;
        m_bulletTime.ActiveBulletTime(false, BulletTime.BulletTimePriority.BulletTimePriority_High);
    }

    //Rewired------------------------------------------------------------
    void FixedUpdate () {
        if( active == false)
        {
            return;
        }
    }

	public override void Do () {
        if (!active || !playerControl.canMove || !Check ())
			return;
        if(m_bChargeZero == true)
        {
            charge = 0;
            m_bChargeZero = false;
        }
		StartCoroutine (DoDash ());
	}

	IEnumerator DoDash () {
        //Rewired 手柄震动---------------------------------------------------
        PlayerControl1.Instance.player.SetVibration(0, motor1Level, motor1duration);
        PlayerControl1.Instance.player.SetVibration(1, motor2Level, motor2duration);

        playerControl.m_bJumpRelease = false;
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dir = (mouseWorldPos - (Vector2)player.transform.position).normalized;

        //Rewired------------------------------------------------------------
        if (rPlayer.GetAxis("MoveHorizontal") != 0 || rPlayer.GetAxis("MoveVertical") != 0)
        {
            if(playerControl.IsKeyBoard() == false)
            {
                dir = new Vector2(rPlayer.GetAxis("MoveHorizontal"), rPlayer.GetAxis("MoveVertical")).normalized;
                dashDir = dir;
            }
        }
        else if (!(playerControl.controlState == PlayerControl1.ControlWay.isKeyboard))
        {
            dir = dashDir;
        }

        if (playerControl.controlState == PlayerControl1.ControlWay.isMobile)
            dir = TouchControl.Instance.finalDashDir.normalized;

        audioSource.PlayOneShot(clip, 0.5f);
        isDashing = true;
        if(m_uaDashStart != null)
        {
            m_uaDashStart.Invoke();
        }
        m_aniCom.PlayerDashStart();

        //冲刺特效
        if (dashParticle != null)
        {
            //gaidao要求这里新的冲刺动画，角度不需要变了。
            GameObject dashP = Instantiate(dashParticle, transform.position, Quaternion.identity, null);
            //GameObject dashP = Instantiate(dashParticle, transform.position, Quaternion.Euler(0, 0, -AngleBetween(Vector2.up, dir)),null);
        }

        //gaidao要求去除变黑
		//if (isShadowDash) {
		//    player.GetComponent<SpriteRenderer> ().color = Color.black;
			//Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("CanCrossFloor"),true);
			
        playerControl.canMove = false;
        playerBody.velocity = dir * DashSpeed;
        if( DashTime <= 0 )
        {
            yield return new WaitForSeconds(0.05f);
        }
        else
        {
            yield return new WaitForSeconds(DashTime);
        }
        charge -= 1;
        m_bulletTime.ActiveBulletTime(false, BulletTime.BulletTimePriority.BulletTimePriority_High);
        isDashing = false;
        if(m_uaDashOver != null)
        {
            m_uaDashOver.Invoke();
        }
        m_aniCom.PlayerDashStop();

        yield return new WaitForSeconds(disableMovementTime - 0.05f);
        playerControl.canMove = true;
        yield return null;

        if (isShadowDash)
        {
            player.GetComponent<SpriteRenderer>().color = Color.white;
        }
        
        transform.rotation = Quaternion.Euler(0, 0, 0);


    }
    public void RegisteDashEvent(UnityAction uaDashStart, UnityAction uaDashOver)
    {
        m_uaDashStart = uaDashStart;
        m_uaDashOver = uaDashOver;
    }

    //抛物线绘制
    void DrawTrajectory()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dir = (mouseWorldPos - (Vector2)player.transform.position).normalized;

        if (playerControl.IsKeyBoard() == false && (rPlayer.GetAxis("MoveHorizontal") != 0 || rPlayer.GetAxis("MoveVertical") != 0))
        {
            dir = new Vector2(rPlayer.GetAxis("MoveHorizontal"), rPlayer.GetAxis("MoveVertical")).normalized;
            dashDir = dir;
        }
        else if (!(playerControl.controlState == PlayerControl1.ControlWay.isKeyboard))
        {
            dir = dashDir;
        }
        lr.enabled = true;

        GameObject target = gameObject;
        lr.SetPositions(PlotTrajectory(target.transform.position, dir * DashSpeed, TrajectoryTimeStep, TrajectoryStepCounts));
    }

    public Vector3 PlotTrajectoryAtTime(Vector3 start, Vector3 startVelocity, float time)
    {
        return start + startVelocity * time + Physics.gravity * playerBody.gravityScale * time * time * 0.5f;
    }

    public Vector3[] PlotTrajectory(Vector3 start, Vector3 startVelocity, float fTimeStep, int stepCounts)
    {
        float maxTime = fTimeStep * stepCounts;
        Vector3[] results = new Vector3[stepCounts];
        Vector3 prev = start;
        for (int i = 1; ; i++)
        {
            float t = fTimeStep * i;
            if (t >= maxTime) break;
            Vector3 pos = PlotTrajectoryAtTime(start, startVelocity, t);
            if (Physics.Linecast(prev, pos)) break;
            prev = pos;
            results[i -1] = pos;
        }
        return results;
    }

    public Vector3[] Plot(Rigidbody2D rigidbody, Vector3 pos, Vector2 velocity, int steps)
    {
        Vector3[] results = new Vector3[steps];
        float timestep = 0.04f;
        Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale;// * timestep;
        Vector2 moveStep = velocity * timestep;

        for (int i = 0; i < steps; ++i)
        {
            moveStep += velocity * Time.fixedDeltaTime;
            velocity += gravityAccel;
            pos += (Vector3)moveStep;
            results[i] = pos;
        }

        return results;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + new Vector3(1, 1, 0), Color.red, 5);
    }
    public static float AngleBetween(Vector2 vectorA, Vector2 vectorB)
    {
        float angle = Vector2.Angle(vectorA, vectorB);
        Vector3 cross = Vector3.Cross(vectorA, vectorB);

        if (cross.z > 0)
        {
            angle = 360 - angle;
        }

        return angle;
    }

    public void DashStart()
    {
        //m_bDashStart = true;
    }
    public void DashEndToIdle()
    {
        //if(playerControl.isTouchingGround == true)
        //{
        //    m_bDashStart = false;
        //}
    }
}

