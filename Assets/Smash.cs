using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smash : Skill {

	// Use this for initialization

	public float smashSpeed=10f;
	public float smashTransitionTime=0.2f;

	public int maxCharge;
	int charge;

	public override void Init()
	{
		charge = maxCharge;
	}

	public override bool Check()
	{
		return charge > 0;
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (playerControl.isTouchingGround) 
		{
			charge = maxCharge;
			StopCoroutine(DoSmash());

		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			Do();
		}
		
	}

	public override void Do()
	{
		if (!active || !playerControl.canMove || !Check())
			return;
		
		playerControl.canMove = false;

		StartCoroutine (DoSmash ());
	}

	IEnumerator DoSmash()
	{
		Debug.Log("Smash!");
		playerBody.velocity=Vector2.zero;
		playerBody.gravityScale=0f;
		yield return new WaitForSeconds(smashTransitionTime);
		playerBody.gravityScale=200f;
		playerBody.velocity = Vector2.down *smashSpeed;
		playerControl.canMove = true;
	}
}
