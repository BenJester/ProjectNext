using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Rewired;

public class Dash : Skill {
	public bool isShadowDash = false;
	public float DashSpeed;
	public float DashDuration;
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
    private Player rPlayer;

    private PlayerStateManager m_stateMgr;

    private UnityAction m_uaDashStart;
    private UnityAction m_uaDashOver;

    private PlayerAnimationComponent m_aniCom;

    private BulletTime m_bulletTime;
    public override void Init () {

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
        if( Mathf.Abs(rPlayer.GetAxis("MoveHorizontal")) > 0 )
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

        //Rewired------------------------------------------------------------
        if ((Input.GetMouseButton (1) && charge >= 1 && currWaitTime >= waitTime) 
            || (rPlayer.GetButton("Dash") && charge >= 1 && currWaitTime >= waitTime)
            || (!(playerControl.controlState == PlayerControl1.ControlWay.isKeyboard) && TouchControl.Instance.dashDrag && charge >= 1 && currWaitTime >= waitTime)) {
            
            //播放动画
            if(!amin.GetCurrentAnimatorStateInfo(0).IsName("Dash_Charging"))
            //amin.CrossFade("Dash_Charging", 0.01f);


            

            //蓄力粒子
            if (_dashChargeParticle == null)
            {
                _dashChargeParticle = Instantiate(dashChargingParticle, transform.position, Quaternion.identity, transform);
            }


            //显示箭头
            if (dashPointer != null)
            {
                Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 dir = (mouseWorldPos - (Vector2)player.transform.position).normalized;
                dashPointer.SetActive(true);
                dashPointer.transform.position = (Vector2)transform.position + dir * 70f;
                dashPointer.transform.localRotation = Quaternion.Euler(0, 0, -AngleBetween(Vector2.up, dir));
            }
            



            playerControl.swap.realWaitTime = playerControl.swap.waitTime;
			playerControl.targetDeltaTime = Time.fixedDeltaTime;
			playerControl.targetTimeScale = Time.timeScale;

            m_bulletTime.ActiveBulletTime(true, BulletTime.BulletTimePriority.BulletTimePriority_High);

            //Time.timeScale = Mathf.Min(Time.timeScale, reducedTimeScale);
            //Time.fixedDeltaTime = reducedTimeScale * playerControl.startDeltaTime;

            //辅助线指示
            DrawTrajectory();

            //播放动态残影
            playerControl.SetColShadow();
		}

        //Rewired------------------------------------------------------------
        if (Input.GetMouseButtonUp (1) || rPlayer.GetButtonUp("Dash"))
        {
            m_bulletTime.ActiveBulletTime(false, BulletTime.BulletTimePriority.BulletTimePriority_High);
            //Do();
            //if (currWaitTime < remainBulletTimeThreshold)
            //{
            //    Time.timeScale = 1f;
            //    playerControl.targetTimeScale = 1f;
            //    Time.fixedDeltaTime = playerControl.startDeltaTime;
            //    playerControl.targetDeltaTime = Time.fixedDeltaTime;
            //} else
            //{
            //    Time.timeScale = 0.1f;
            //    playerControl.targetTimeScale = 0.1f;
            //    Time.fixedDeltaTime = playerControl.startDeltaTime * 0.1f;
            //    playerControl.targetDeltaTime = playerControl.startDeltaTime * 0.1f;
            //    StartCoroutine(CancelBulletTime());
            //}
            //currWaitTime = 0;

            ////辅助线取消
            //lr.enabled = false;
            ////蓄力冲刺特效取消[是不是不生成比较有效率？]
            //if (_dashChargeParticle != null) Destroy(_dashChargeParticle);
            //if (dashPointer != null) dashPointer.SetActive(false);

        }
		if (playerControl.isTouchingGround) {
			charge = maxCharge;
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
        yield return new WaitForSecondsRealtime(remainBulletTimeDuration);
        playerControl.targetTimeScale = 1f;
        playerControl.targetDeltaTime = playerControl.startDeltaTime * 1f;
    }

    //Rewired------------------------------------------------------------
    void FixedUpdate () {
		if (Input.GetMouseButton (1)|| (rPlayer != null && rPlayer.GetButton("Dash")) || (playerControl.controlState == PlayerControl1.ControlWay.isMobile && TouchControl.Instance.dashDrag))
        {
            if(currWaitTime == 0)
            {
                m_aniCom.PlayerDashCharging();
            }
            currWaitTime += 1;
        }
        if (Input.GetMouseButtonDown(1)|| rPlayer.GetButtonDown("Dash"))
            playerControl.swap.curr = 0f;
    }

	public override void Do () {
        if (!active || !playerControl.canMove || !Check ())
			return;

		//playerControl.canMove = false;
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

        //反正Input.GetAxis也得不到东西，就给keyboard配置rewired的数据然后通过MoveHorizontal进行处理即可。
        //if (playerControl.controlState == PlayerControl1.ControlWay.isKeyboard)
        //    dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        if (playerControl.controlState == PlayerControl1.ControlWay.isMobile)
            dir = TouchControl.Instance.finalDashDir.normalized;

        audioSource.PlayOneShot(clip, 0.5f);
        isDashing = true;
        if(m_uaDashStart != null)
        {
            m_uaDashStart.Invoke();
        }
        Debug.Log("dash start");
        m_aniCom.PlayerDashStart();

        m_stateMgr.SetPlayerState(PlayerStateDefine.PlayerState_Typ.playerState_Dash);

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
			
		
        float curr = 0f;
        //		while (curr < pauseDuration) 
        //		{
        //			playerBody.velocity = Vector2.zero;
        //			curr += Time.deltaTime;
        //			yield return new WaitForEndOfFrame ();
        //		}
        //		curr = 0f;
        //		float h = (Input.GetKey(KeyCode.D) ? 1 : 0) + (Input.GetKey(KeyCode.A) ? -1 : 0);
        //		float v = (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -1 : 0);
        //		Vector2 dir = new Vector2(h, v).normalized;
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
        isDashing = false;
        if(m_uaDashOver != null)
        {
            m_uaDashOver.Invoke();
        }
        Debug.Log("dash over");
        m_aniCom.PlayerDashStop();

        m_stateMgr.SetPlayerState(PlayerStateDefine.PlayerState_Typ.PlayerState_None);
        yield return new WaitForSeconds(disableMovementTime - 0.05f);
        playerControl.canMove = true;
        yield return null;
        //      while (curr < DashDuration) {
        //          //playerBody.velocity = dir * DashSpeed;
        //          curr += Time.deltaTime;
        //	yield return new WaitForEndOfFrame ();
        //}

        //playerControl.canMove = true;
        //      playerBody.velocity = dir * playerControl.speed;
        //      curr = 0f;
        //      playerControl.canMove = true;
        //      while (curr < CoyoteDuration)
        //      {
        //          playerBody.gravityScale = 75f;
        //          curr += Time.deltaTime;
        //          yield return new WaitForEndOfFrame();
        //      }
        //      playerBody.gravityScale = gravity;

        if (isShadowDash)
        {
            //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("CanCrossFloor"),false);
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

        GameObject target = gameObject;// playerControl.swap.col.gameObject;
        //lr.SetPositions(Plot(playerBody,
        //                        target.transform.position,
        //                        dir * DashSpeed,
        //                        24));
        lr.SetPositions(PlotTrajectory(target.transform.position, dir * DashSpeed, TrajectoryTimeStep, TrajectoryStepCounts));
        //PlotTrajectory(target.transform.position, dir * DashSpeed, 0.02f,1);
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
            //Debug.DrawLine(prev, pos, Color.red);
            prev = pos;
            results[i -1] = pos;
        }
        return results;
    }

    public Vector3[] Plot(Rigidbody2D rigidbody, Vector3 pos, Vector2 velocity, int steps)
    {
        Vector3[] results = new Vector3[steps];

        float timestep = 0.04f;
        //timestep = Time.deltaTime / Time.timeScale;
        //Debug.Log(timestep);/// (1f * Physics2D.velocityIterations);
        Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale;// * timestep;
        Vector2 moveStep = velocity * timestep;

        for (int i = 0; i < steps; ++i)
        {
            moveStep += velocity * Time.fixedDeltaTime;
            velocity += gravityAccel;
            //moveStep *= drag;
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
}

