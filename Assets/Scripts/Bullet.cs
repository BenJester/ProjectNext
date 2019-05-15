using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	Rigidbody2D rb;
	public float lifespan;
	int age = 0;
	GameObject player;
	Rigidbody2D playerBody;
	void Start () {
		player = GameObject.FindWithTag ("player");
		playerBody = player.GetComponent<Rigidbody2D> ();
	}

	void Update () {
		UpdateLife ();
	}

	void UpdateLife() {
		age += 1;
		if (age > lifespan)
			Destroy (gameObject);
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
			Destroy (gameObject);

		} else if (col.CompareTag ("floor")) {
			Destroy (gameObject);
		}

	}
}

