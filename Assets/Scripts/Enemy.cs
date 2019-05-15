using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour{

	// Use this for initialization
	Thing thing;
	public float dropKillSpeed;
	Goal goal;

	void Start () {
		thing = GetComponent<Thing> ();
		goal = GameObject.FindGameObjectWithTag ("goal").GetComponent<Goal>();
		goal.enemyCount += 1;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D (Collision2D col) {
		if (thing.prevVelocity.y < -dropKillSpeed) {
			thing.collider.enabled = false;
			thing.Die ();
		}
	}

}
