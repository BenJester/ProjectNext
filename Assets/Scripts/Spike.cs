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

	//void OnCollisionEnter2D(Collision2D col) {
	//	if (col.gameObject.CompareTag("thing") || col.gameObject.CompareTag("player")) {
	//		Thing colThing = col.gameObject.GetComponent<Thing> ();
	//		if (colThing.type == Type.enemy) {
 //               colThing.GetComponent<Enemy>().TakeDamage(1);
	//		}
 //           if (colThing.type == Type.player)
 //           {
 //               colThing.Die();
 //               StartCoroutine(colThing.GetComponent<PlayerControl1>().DelayRestart());
 //           }
	//	}
	//}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("thing") || collision.gameObject.CompareTag("player"))
        {
            Thing colThing = collision.gameObject.GetComponent<Thing>();
            if (colThing.type == Type.enemy)
            {
                colThing.GetComponent<Enemy>().TakeDamage(1);
            }
            if (colThing.type == Type.player)
            {
                colThing.Die();
                StartCoroutine(colThing.GetComponent<PlayerControl1>().DelayRestart());
            }
        }
    }
}
