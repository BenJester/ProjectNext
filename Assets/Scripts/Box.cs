﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

	public float killDropSpeed;
	public float killRange;
	Rigidbody2D body;
	Thing _boxThing;
	Vector2 prevVelocity;

	void Start () {
		body = GetComponent<Rigidbody2D> ();
		_boxThing = GetComponent<Thing> ();	
	}
	
	void Update () {
		prevVelocity = body.velocity;
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.CompareTag("thing")) {
			Thing colThing = col.gameObject.GetComponent<Thing> ();
			if (colThing.type == Type.enemy && prevVelocity.y < -killDropSpeed && _boxThing.GetLowerY() <= colThing.GetUpperY() + killRange) {
                if( colThing.GetLowerY() > _boxThing.GetLowerY() )
                {
                    //碰撞物在箱子上面。就不处理。
                }
                else
                {
                    colThing.Die();
                }
			}
            else
            {
                //Debug.Assert(false);
            }
            if(body != null)
            {
                body.velocity = prevVelocity;
            }
		}
	}
}
