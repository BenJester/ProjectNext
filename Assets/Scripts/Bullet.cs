﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float lifespan;
	public bool active = true;

	public enum BulletType {
		slow = 0,
		fast = 1,
	}
	
	protected SpriteRenderer sr;
	protected Rigidbody2D body;
	public Vector3 smokeOffset;
	protected int age = 0;
	protected GameObject player;
	protected Rigidbody2D playerBody;
	protected Collider2D collider;
	public GameObject dashParticle;
	public Color slowColor;
	public Color fastColor;
	PlayerControl1 playerControl;

	protected void Start () {
		//anim=GetComponent<Animator>();
		
		sr = GetComponent<SpriteRenderer> ();
		player = GameObject.FindWithTag ("player");
		playerControl = player.GetComponent<PlayerControl1> ();
		playerBody = player.GetComponent<Rigidbody2D> ();
		//Rewind.Instance.bullets.Add (gameObject);
		//Rewind.Instance.bulletBody.Add (GetComponent<Rigidbody2D> ());
		body = GetComponent<Rigidbody2D> ();
		collider = GetComponent<Collider2D> ();
		
	}

	public void Update () {
		UpdateLife ();
	}

	public void UpdateLife () {
		age += 1;
		if (age > lifespan) {
			Deactivate ();
		}
	}

	public void Activate () {
		active = true;
		sr.enabled = true;
		collider.enabled = true;
	}

	public void Deactivate () {
		active = false;
		sr.enabled = false;
		collider.enabled = false;
		body.velocity = Vector2.zero;
	}

	public void SetBulletType (BulletType type) {

		
		switch (type) {
			
			case BulletType.slow:
				GetComponent<Animator>().CrossFade("Slow",0.001f);
				TrailRenderer tr =GetComponent<TrailRenderer>(); 
				tr.startColor=slowColor;
				tr.endColor=slowColor;
				tr.time=0.3f;
				break;
			case BulletType.fast:
				GetComponent<Animator>().CrossFade("Fast",0.001f);
				TrailRenderer tr2 =GetComponent<TrailRenderer>(); 
				tr2.startColor=fastColor;
				tr2.endColor=fastColor;
				tr2.time=0.3f;
				break;

		}
	}

	public virtual void OnTriggerEnter2D (Collider2D col) {

		if (col.CompareTag ("thing")) {
			playerControl.swap.col = col;
			playerControl.swap.Do ();
			playerControl.doubleSwap = true;

			//			Rigidbody2D thingBody = col.gameObject.GetComponent<Rigidbody2D> ();
			//			Thing thing = col.gameObject.GetComponent<Thing> ();
			//			Vector3 pos = player.transform.position;
			//			Vector3 thingPos = col.transform.position;
			//			float playerRadiusY = player.GetComponent<BoxCollider2D> ().size.y / 2f;
			//			float heightDiff = (col.GetComponent<BoxCollider2D> ().size.y * col.transform.localScale.y - playerRadiusY * 2f) / 2f;
			//
			//			if (thing.leftX < player.transform.position.x && thing.rightX > player.transform.position.x && thing.lowerY > player.transform.position.y && thing.lowerY < player.transform.position.y + playerRadiusY + 10f) {
			//				if () {
			//				Vector3 temp = col.gameObject.transform.position;
			//				col.gameObject.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y - playerRadiusY + (thing.upperY - thing.lowerY) / 2f, player.transform.position.z);
			//				player.transform.position = new Vector3 (temp.x, col.gameObject.transform.position.y + playerRadiusY + (thing.upperY - thing.lowerY) / 2f, player.transform.position.z);
			//
			//				
			//				
			//					//	} else {
			//					Vector3 temp = player.transform.position;
			//					player.transform.position = new Vector3 (col.gameObject.transform.position.x, col.gameObject.transform.position.y + player.GetComponent<BoxCollider2D> ().size.y / 2f - (thing.upperY - thing.lowerY) / 2f, player.transform.position.z);
			//					col.gameObject.transform.position = new Vector3 (temp.x, player.gameObject.transform.position.y + player.GetComponent<BoxCollider2D> ().size.y / 2f + (thing.upperY - thing.lowerY) / 2f, player.transform.position.z);
			//				}
			//
			//			} else {
			//				Vector3 tempPos = new Vector3 (pos.x, pos.y + heightDiff, pos.z);
			// 				GameObject par1 = Instantiate(dashParticle,player.transform.position+smokeOffset,Quaternion.identity);
			// 				GameObject par2 = Instantiate(dashParticle,thingPos+smokeOffset,Quaternion.identity);
			// 				Destroy(par1,1f);
			// 				Destroy(par2,1f);
			// //				player.transform.position = new Vector3 (thingPos.x, thingPos.y - heightDiff, thingPos.z);
			// //				col.gameObject.transform.position = tempPos;
			// //				//交换的瞬间
			// //				
			// 				PostEffectManager.instance.Blink(0.03f);
			//			}
			//
			//
			//
			//
			//			Vector2 tempV = playerBody.velocity;
			//			playerBody.velocity = thingBody.velocity;
			//			thingBody.velocity = tempV;
			Deactivate ();

		} else if (col.CompareTag ("floor")) {
			Deactivate ();
		}

	}
}