using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ben;
public class EnemyBullet : Bullet {

//	public bool active;
//	SpriteRenderer sr;
//	Collider2D collider;

    public int damage;
    public bool friendly;
	PlayerControl pc;
    public bool floorCollide = true;
    float startSpeed;
    //	GameObject player;
    void Start () {
		base.Start();
//		player = GameObject.FindWithTag ("player");
		pc = player.GetComponent<PlayerControl> ();
        //		sr = GetComponent<SpriteRenderer> ();
        //		collider = GetComponent<Collider2D> ();
        startSpeed = body.velocity.magnitude;

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
	public void OnTriggerEnter2D(Collider2D col) {

		if (col.CompareTag ("thing")) {
			if (col.GetComponent<Thing> ().type != Type.box && col.GetComponent<Thing>().type != Type.invincible)
            {
                if (col.GetComponent<Enemy>() != null)
                {
                    col.GetComponent<Enemy>().TakeDamage(damage);
                }
            }
            col.GetComponent<Thing>().TriggerMethod?.Invoke();

            Deactivate();


        } else if (col.CompareTag ("player") && !friendly) {
			
			pc.Die ();
			Deactivate ();
		}
		else if (col.CompareTag ("floor") && floorCollide) {
			Deactivate ();
		}
        else if (col.CompareTag("metalShield"))
        {
            body.velocity = -body.velocity.normalized * startSpeed * 2f;
        }
    }
}
