using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_Portal : MonoBehaviour {

	public Mech_Portal target;
    public Vector2 dir;
    private Vector2 targetPos;

    public bool onlyPlayer = false;
    [HideInInspector]public bool canPort = true;
	void Start () {
        canPort = true;
        if (target != null) {
            targetPos = target.transform.position;
        }
        
	}
	
	private void OnTriggerEnter2D(Collider2D col) {

        if (onlyPlayer && col.tag!="player") {
            return;
        }

        if (!target || !canPort)
            return;
        if (target.dir != Vector2.zero)
        {
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            float speed = rb.velocity.magnitude;
            rb.velocity = target.dir * speed;
        }
        col.transform.position = targetPos;

        SetNoPort();
        Invoke("SetCanPort", 0.05f);
	}
    private void SetCanPort()
    {
        target.canPort = true;
        canPort = true;
    }
    private void SetNoPort()
    {
        target.canPort = false;
        canPort = false;
    }
}
