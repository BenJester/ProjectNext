using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.CompareTag("thing") || col.gameObject.CompareTag("player")) {
			Thing colThing = col.gameObject.GetComponent<Thing> ();
			if (colThing.type == Type.enemy ||  colThing.type == Type.player) {
				colThing.Die ();
			}
		}
	}
}
