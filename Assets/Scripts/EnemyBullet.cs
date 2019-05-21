using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet {

//	public bool active;
//	SpriteRenderer sr;
//	Collider2D collider;
	PlayerControl pc;
//	GameObject player;
	void Start () {
		base.Start();
//		player = GameObject.FindWithTag ("player");
		pc = player.GetComponent<PlayerControl> ();
//		sr = GetComponent<SpriteRenderer> ();
//		collider = GetComponent<Collider2D> ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

//	public void Activate() {
//		active = true;
//		sr.enabled = true;
//		collider.enabled = true;
//	}
//
//	public void Deactivate() {
//		active = false;
//		sr.enabled = false;
//		collider.enabled = false;
//	}
//
	public override void OnTriggerEnter2D(Collider2D col) {

		if (col.CompareTag ("thing")) {
			if (col.GetComponent<Thing> ().type != Type.box)
				col.GetComponent<Thing> ().Die ();
			Deactivate ();
				

		} else if (col.CompareTag ("player")) {
			
			pc.Die ();
			Deactivate ();
		}
		else if (col.CompareTag ("floor")) {
			Deactivate ();
		}

	}
}
