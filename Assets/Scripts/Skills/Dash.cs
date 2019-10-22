using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public int maxCharge;
	int charge;
    public float multiplier;

    public GameObject dashParticle;
    public GameObject dashPointer;
    public GameObject dashChargingParticle;
    private GameObject _dashChargeParticle;

    public bool isDashing = false;

	LineRenderer lr;
    Animator amin;

	public override void Init () {
		lr = GetComponent<LineRenderer> ();
        amin = GetComponent<Animator>();
        charge = maxCharge;
	}

	public override bool Check () {
		return charge > 0;
	}

	void Update () {
        //Debug.Log(Time.timeScale);

        //播放冲刺情况下的动画
        if (isDashing)
        {
            //冲刺中动画
            if (!amin.GetCurrentAnimatorStateInfo(0).IsName("Dashing"))
            {
                //amin.CrossFade("Dashing", 0.01f);
            }
            transform.rotation = Quaternion.Euler(0, 0, -AngleBetween(Vector2.up, GetComponent<Rigidbody2D>().velocity));
        }

        if (Input.GetMouseButton (1) && charge >= 1 && currWaitTime >= waitTime) {
            
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
			Time.timeScale = Mathf.Min (Time.timeScale, reducedTimeScale);
			Time.fixedDeltaTime = reducedTimeScale * playerControl.startDeltaTime;
			playerControl.targetDeltaTime = Time.fixedDeltaTime;
			playerControl.targetTimeScale = Time.timeScale;


            //辅助线指示
            DrawTrajectory();
		}

		if (Input.GetMouseButtonUp (1)) {
            Do();
            if (currWaitTime < remainBulletTimeThreshold)
            {
                Time.timeScale = 1f;
                playerControl.targetTimeScale = 1f;
                Time.fixedDeltaTime = playerControl.startDeltaTime;
                playerControl.targetDeltaTime = Time.fixedDeltaTime;
            } else
            {
                Time.timeScale = 0.1f;
                playerControl.targetTimeScale = 0.1f;
                Time.fixedDeltaTime = playerControl.startDeltaTime * 0.1f;
                playerControl.targetDeltaTime = playerControl.startDeltaTime * 0.1f;
            }
            currWaitTime = 0;

            //辅助线取消
            lr.enabled = false;
            //蓄力冲刺特效取消[是不是不生成比较有效率？]
            if (_dashChargeParticle != null) Destroy(_dashChargeParticle);
            if (dashPointer != null) dashPointer.SetActive(false);

        }
		if (playerControl.isTouchingGround) {
			charge = maxCharge;
		}
	}

	void FixedUpdate () {
		if (Input.GetMouseButton (1))
			currWaitTime += 1;
        if (Input.GetMouseButtonDown(1))
            playerControl.swap.curr = 0f;
    }

	public override void Do () {
        if (!active || !playerControl.canMove || !Check ())
			return;

		//playerControl.canMove = false;
		StartCoroutine (DoDash ());
	}

	IEnumerator DoDash () {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mouseWorldPos - (Vector2)player.transform.position).normalized;


        isDashing = true;


        //冲刺特效
        if (dashParticle != null)
        {
            GameObject dashP = Instantiate(dashParticle, transform.position, Quaternion.Euler(0, 0, -AngleBetween(Vector2.up, dir)),null);
        }

		if (isShadowDash) {
		    player.GetComponent<SpriteRenderer> ().color = Color.black;
			//Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("CanCrossFloor"),true);
			
			
		}

        
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
        
        playerBody.velocity = dir * DashSpeed;
        yield return new WaitForSeconds(0.05f);
        charge -= 1;
        isDashing = false;
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


    //抛物线绘制
    void DrawTrajectory()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mouseWorldPos - (Vector2)player.transform.position).normalized;
        lr.enabled = true;

        if (!playerControl.swap.delaying)
        {

            //lr.SetPosition(0, transform.position);
            //for (int i = 1; i < 25; i ++)
            //{
            //    lr.SetPosition(i, transform.position + (Vector3)dir * 170);
            //}
            GameObject target = gameObject;// playerControl.swap.col.gameObject;
            lr.SetPositions(Plot(playerControl.GetComponent<Rigidbody2D>(),
                                 target.transform.position,
                                 dir * DashSpeed,
                                 5));
            for (int i = 5; i < lr.positionCount; i++)
            {
                lr.SetPosition(i, lr.GetPosition(4));
            }
        }
        else
        {
            GameObject target = gameObject;// playerControl.swap.col.gameObject;
            lr.SetPositions(Plot(playerControl.swap.col.GetComponent<Rigidbody2D>(),
                                 target.transform.position,
                                 dir * DashSpeed,
                                 25));
        }
    }

    public Vector3[] Plot(Rigidbody2D rigidbody, Vector3 pos, Vector2 velocity, int steps)
    {
        Vector3[] results = new Vector3[steps];
        
        float timestep = 0.05f;
        Debug.Log(timestep);/// (1f * Physics2D.velocityIterations);
        Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale;// * timestep;
        Vector2 moveStep = velocity * timestep;

        for (int i = 0; i < steps; ++i)
        {
            moveStep += velocity * Time.fixedDeltaTime;
            velocity += gravityAccel;
            //moveStep *= drag;
            pos += (Vector3) moveStep;
            results[i] = pos;
        }

        return results;
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

