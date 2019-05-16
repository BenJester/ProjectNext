using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type {
	player = 1,
	enemy = 2,
	box = 3
}

public class Thing : MonoBehaviour {
	
	public Type type;
	public float lowerY;
	public float upperY;

	public BoxCollider2D collider;
	public Rigidbody2D body;
	public Vector2 prevVelocity;

	Vector3 originalScale;
	Goal goal;
	public bool dead = false;

	public void Awake () {
		originalScale = transform.localScale;
		collider = GetComponent<BoxCollider2D> ();
		body = GetComponent<Rigidbody2D> ();
		goal = GameObject.FindGameObjectWithTag ("goal").GetComponent<Goal>();
		switch (type) {
			case Type.box:
				Rewind.obj.Add (gameObject);
				break;
			case Type.enemy:
				Rewind.enemies.Add (gameObject);
				break;
			default:
				break;
		}

	}
	
	public void Update () {
		lowerY = transform.position.y - collider.size.y / 2f;
		upperY = transform.position.y + collider.size.y / 2f;
	}

	void FixedUpdate () {
		prevVelocity = body.velocity;
	}

	public void Die() {
		if (dead)
			return;
		dead = true;
		if (type == Type.enemy)
			goal.enemyCount -= 1;

		StartCoroutine (ScaleDown(0.2f));
	}

	IEnumerator ScaleDown(float duration) {
		gameObject.GetComponent<BoxCollider2D>().enabled = false;
		gameObject.GetComponent<SpriteRenderer>().enabled = false;
		gameObject.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Static;

		while (transform.localScale.x >= 0.02) {
			float perc = Time.deltaTime / duration;
			transform.localScale -= perc * originalScale;
			yield return new WaitForEndOfFrame ();
		}
	}

	IEnumerator ScaleUp(float duration) {
		gameObject.GetComponent<BoxCollider2D>().enabled = true;
		gameObject.GetComponent<SpriteRenderer>().enabled = true;
		gameObject.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic;
		while (transform.localScale.x <= originalScale.x) {
			float perc = Time.deltaTime / duration;
			transform.localScale += perc * originalScale;
			yield return new WaitForEndOfFrame ();
		}
		transform.localScale = originalScale;

	}

	public void Revive() {
		dead = false;
		if (type == Type.enemy)
			goal.enemyCount += 1;

		StartCoroutine (ScaleUp(0.2f));
	}
}
