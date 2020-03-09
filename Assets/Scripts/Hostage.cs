using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hostage : MonoBehaviour {

	Thing thing;
	public float dropKillSpeed;
	Goal goal;
    public float speed;
    public float jumpSpeed;

    Rigidbody2D rb;

	void Start () {
		thing = GetComponent<Thing> ();
		goal = GameObject.FindGameObjectWithTag ("goal").GetComponent<Goal>();
		goal.hostageList.Add (GetComponent<Thing> ());
        rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update () {
		if (thing.upperY<-600f)
		{
			thing.Die();
		}
	}

    bool detectObstacle()
    {

        return false;
    }

    void Jump()
    {
        if (thing.touchingFloor())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (speed != 0f)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        if (thing.touchingWallRight())
            Jump();
    }
    void OnCollisionEnter2D (Collision2D col) {

		if (thing.prevVelocity.y < -dropKillSpeed && col.transform.position.y < transform.position.y) {
			thing.collider.enabled = false;
			thing.Die ();
		}
	}
}
