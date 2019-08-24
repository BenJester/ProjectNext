using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Skill {

	public float DashSpeed;
	public float DashDuration;
	public float pauseDuration;

	public override void Do()
	{
		if (!active || !playerControl.canMove)
			return;
		
		playerControl.canMove = false;

		StartCoroutine (DoDash ());
	}

	IEnumerator DoDash()
	{
		
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
		Vector2 dir =  (mouseWorldPos - (Vector2) player.transform.position).normalized;
		playerBody.velocity = dir * DashSpeed;
		while (curr < DashDuration) 
		{
			curr += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		playerBody.velocity = dir * playerControl.speed;
		playerControl.canMove = true;
	}
}
