using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ben;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBullet_Homing : MonoBehaviour
{
    // Start is called before the first frame update

    [Space]
    [Header("Homing Bullet Setting")]
    public bool floorCollide=true;
    public bool playerAsTarget=true;
    public Transform target;
    public float speed =250f;
    public float rotateSpeed = 200f;

    private Rigidbody2D rb;

    PlayerControl pc;
    void Start()
    {
        //base.Start();
		pc = GameObject.FindWithTag("player").GetComponent<PlayerControl> ();
        rb =GetComponent<Rigidbody2D>();
        
        //自动设置为玩家
        if(playerAsTarget) target = pc.transform;
    }

   
    void FixedUpdate()
    {
        //都是朝上发射

        Vector2 direcion = ((Vector2)target.position - rb.position).normalized;

        float rotateAmount = Vector3.Cross(direcion,transform.up).z;
        rb.angularVelocity = -rotateAmount*rotateSpeed;
        rb.velocity = transform.up*speed;

        
        //rb.velocity = transform.up * speed;
    }

    public void OnTriggerEnter2D(Collider2D col) {

		if (col.CompareTag ("thing")) {
			if (col.GetComponent<Thing> ().type != Type.box)
				col.GetComponent<Thing> ().Die ();
			Deactivate ();
				

		} else if (col.CompareTag ("player")) {
			
			pc.Die ();
			Deactivate ();
		}
		else if (col.CompareTag ("floor") && floorCollide) {
			Deactivate ();
		}

	}

    public void Deactivate () {
		
        //active = false;
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<Collider2D>().enabled = false;
		//rb.velocity = Vector2.zero;
	}
}
