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
	public int maxCharge;
	int charge;
    public float multiplier;

	LineRenderer lr;

	public override void Init () {
		lr = GetComponent<LineRenderer> ();
		charge = maxCharge;
	}

	public override bool Check () {
		return charge > 0;
	}

	void Update () {
		//Debug.Log(Time.timeScale);
		if (Input.GetMouseButton (1) && charge >= 1 && currWaitTime >= waitTime) {
			playerControl.swap.realWaitTime = playerControl.swap.waitTime;
			Time.timeScale = Mathf.Min (Time.timeScale, reducedTimeScale);
			Time.fixedDeltaTime = reducedTimeScale * playerControl.startDeltaTime;
			playerControl.targetDeltaTime = Time.fixedDeltaTime;
			playerControl.targetTimeScale = Time.timeScale;


            //辅助线指示
            DrawTrajectory();
		}

		if (Input.GetMouseButtonUp (1)) {
			currWaitTime = 0;
			Do ();
			Time.timeScale = 1f;
			playerControl.targetTimeScale = 1f;
			Time.fixedDeltaTime = playerControl.startDeltaTime;
			playerControl.targetDeltaTime = Time.fixedDeltaTime;

            //辅助线取消
            lr.enabled = false;
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

		playerControl.canMove = false;

		StartCoroutine (DoDash ());
	}

	IEnumerator DoDash () {

		if (isShadowDash) {
		    player.GetComponent<SpriteRenderer> ().color = Color.black;
			//Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("CanCrossFloor"),true);
			
			
		}
        charge -= 1;
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
		Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 dir = (mouseWorldPos - (Vector2) player.transform.position).normalized;
        while (curr < DashDuration) {
            playerBody.velocity = dir * DashSpeed;
            curr += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		
		playerBody.velocity = dir * playerControl.speed;
        curr = 0f;
        playerControl.canMove = true;
        while (curr < CoyoteDuration)
        {
            playerBody.gravityScale = 75f;
            curr += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        playerBody.gravityScale = gravity;

        if (isShadowDash) {
			//Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("CanCrossFloor"),false);
			
			player.GetComponent<SpriteRenderer> ().color = Color.white;
		}
		
	}

    void DrawTrajectory()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mouseWorldPos - (Vector2)player.transform.position).normalized;
        lr.enabled = true;

        if (!playerControl.swap.delaying)
        {
            lr.SetPosition(0, transform.position);
            for (int i = 1; i < 25; i ++)
            {
                lr.SetPosition(i, transform.position + (Vector3)dir * 170);
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
}