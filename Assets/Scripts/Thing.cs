﻿using System.Collections;
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

	Goal goal;
	bool dead = false;

	public void Start () {
		collider = GetComponent<BoxCollider2D> ();
		body = GetComponent<Rigidbody2D> ();
		goal = GameObject.FindGameObjectWithTag ("goal").GetComponent<Goal>();
		switch (type) {
			case Type.box:
				ActorManager.obj.Add (gameObject);
				break;
			case Type.enemy:
				ActorManager.enemies.Add (gameObject);
				break;
			default:
				break;
		}

		ActorManager.enemies.Add (gameObject);

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
		Vector3 originalScale = transform.localScale;
		while (transform.localScale.x >= 0.02) {
			float perc = Time.deltaTime / duration;
			transform.localScale -= perc * originalScale;
			yield return new WaitForEndOfFrame ();
		}
		Destroy (gameObject);
	}
}
