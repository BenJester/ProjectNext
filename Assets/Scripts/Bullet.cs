using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	
	public float lifespan;
	public bool active;

	SpriteRenderer sr;
	Rigidbody2D body;
	int age = 0;
	GameObject player;
	Rigidbody2D playerBody;
	Collider2D collider;

	void Awake () {
		sr = GetComponent<SpriteRenderer> ();
		player = GameObject.FindWithTag ("player");
		playerBody = player.GetComponent<Rigidbody2D> ();
		Rewind.bullets.Add (gameObject);
		Rewind.bulletBody.Add(GetComponent<Rigidbody2D>());
		body = GetComponent<Rigidbody2D> ();
		collider = GetComponent<Collider2D> ();
	}

	void Update () {
		UpdateLife ();
	}

	void UpdateLife() {
		age += 1;
		if (age > lifespan) {
			Deactivate();
		}
	}

	public void Activate() {
		active = true;
		sr.enabled = true;
		collider.enabled = true;
	}

	public void Deactivate() {
		active = false;
		sr.enabled = false;
		collider.enabled = false;
	}

	void OnTriggerEnter2D(Collider2D col) {
		
		if (col.CompareTag ("thing")) {
			Rigidbody2D thingBody = col.gameObject.GetComponent<Rigidbody2D> ();

			float heightDiff = (col.GetComponent<BoxCollider2D> ().size.y * col.transform.localScale.y - player.GetComponent<BoxCollider2D> ().size.y) / 2f;
			Vector3 pos = player.transform.position;
			Vector3 thingPos = col.transform.position;

			Vector3 tempPos = new Vector3 (pos.x, pos.y + heightDiff, pos.z);
			player.transform.position = new Vector3 (thingPos.x, thingPos.y - heightDiff, thingPos.z);
			col.gameObject.transform.position = tempPos;

			Vector2 tempV = playerBody.velocity;
			playerBody.velocity = thingBody.velocity;
			thingBody.velocity = tempV;
			Deactivate ();

		} else if (col.CompareTag ("floor")) {
			Deactivate ();
		}

	}
}

