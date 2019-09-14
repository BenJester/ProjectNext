using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangedDispearThing : MonoBehaviour {
    private Vector2 originPos;
    public bool active = true;

    void Start () {
        originPos = transform.position;
    }

    // Update is called once per frame
    void Update () {
        if (originPos != (Vector2) transform.position) {
            Deactivate();
        }
    }

    public void Deactivate () {
		active = false;
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
	}

}