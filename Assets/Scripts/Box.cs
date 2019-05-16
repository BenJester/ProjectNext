using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

	public float killDropSpeed;
	public float killRange;
	Rigidbody2D body;
	Thing thing;
	Vector2 prevVelocity;

	void Start () {
		body = GetComponent<Rigidbody2D> ();
		thing = GetComponent<Thing> ();	
	}
	
	void FixedUpdate () {
		prevVelocity = body.velocity;
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.CompareTag("thing")) {
			Thing colThing = col.gameObject.GetComponent<Thing> ();
			if (colThing.type == Type.enemy && prevVelocity.y < -killDropSpeed && thing.lowerY <= colThing.upperY + killRange) {
				colThing.Die ();
			}
			body.velocity = prevVelocity;
		}
	}
}
