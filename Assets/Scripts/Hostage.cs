using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hostage : MonoBehaviour {

	Thing thing;
	public float dropKillSpeed;
	Goal goal;

	void Start () {
		thing = GetComponent<Thing> ();
		goal = GameObject.FindGameObjectWithTag ("goal").GetComponent<Goal>();
		goal.hostageList.Add (GetComponent<Thing> ());
	}

	// Update is called once per frame
	void Update () {
		if (thing.upperY<-600f)
		{
			thing.Die();
		}
	}

	void OnCollisionEnter2D (Collision2D col) {

		if (thing.prevVelocity.y < -dropKillSpeed && col.transform.position.y < transform.position.y) {
			thing.collider.enabled = false;
			thing.Die ();
		}
	}
}
