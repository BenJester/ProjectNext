using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Skill {

	public bool isShadowDash = false;
	public float DashSpeed;
	public float DashDuration;
	public float pauseDuration;
    public float reducedTimeScale;
    public int waitTime;
    public int currWaitTime;
    public int maxCharge;
	int charge;

	public override void Init () {
		charge = maxCharge;
	}

	public override bool Check () {
		return charge > 0;
	}

	void Update () {
        //Debug.Log(Time.timeScale);
        if (Input.GetMouseButton(1) && charge >= 1 && currWaitTime >= waitTime)
        {
            playerControl.swap.realWaitTime = playerControl.swap.waitTime;
            Time.timeScale = Mathf.Min(Time.timeScale, reducedTimeScale);
            Time.fixedDeltaTime = reducedTimeScale * playerControl.startDeltaTime;
            playerControl.targetDeltaTime = Time.fixedDeltaTime;
            playerControl.targetTimeScale = Time.timeScale;
        }

        if (Input.GetMouseButtonUp(1))
        {
            currWaitTime = 0;
            Do();
            Time.timeScale = 1f;
            playerControl.targetTimeScale = 1f;
            Time.fixedDeltaTime = playerControl.startDeltaTime;
            playerControl.targetDeltaTime = Time.fixedDeltaTime;
        }
        if (playerControl.isTouchingGround) {
			charge = maxCharge;
		}
	}
    void FixedUpdate()
    {
        if (Input.GetMouseButton(1))
            currWaitTime += 1;
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
			player.GetComponent<Collider2D> ().enabled = false;
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
		Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 dir = (mouseWorldPos - (Vector2) player.transform.position).normalized;
		playerBody.velocity = dir * DashSpeed;
		while (curr < DashDuration) {
			curr += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		charge -= 1;
		playerBody.velocity = dir * playerControl.speed;
		if (isShadowDash) {
			player.GetComponent<Collider2D> ().enabled = true;
			player.GetComponent<SpriteRenderer> ().color = Color.white;
		}
		playerControl.canMove = true;
	}
}