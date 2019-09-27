using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_IronFoot : Enemy_Base {

	// Use this for initialization



	Animator anim;
	private float velocityY;
	private Rigidbody2D rb;
	public float threshold=50f;
	public bool isFalling=false;

	private void Awake() {
		anim=GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (rb.velocity.y<=-threshold)
		{
			Falling(true);
		}else if(rb.velocity.y>=-5) Falling(false);
	}
	
	void Falling(bool isFall){
		if(isFall){
			anim.CrossFade("Enemy_IronFoot",0.01f);
			rb.gravityScale=0.1f;
			rb.drag=0.2f;
		}else
		{
			anim.CrossFade("Enemy_IronFoot_idle",0.01f);
			rb.gravityScale=200;
			rb.drag=0f;
		}
	}



}
