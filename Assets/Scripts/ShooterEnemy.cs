using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy {

	// Use this for initialization
	public bool faceRight;
	public GameObject bullet;
	public float bulletSpeed;
	public float shootInterval = 1f;

	int count;

	void Start () {
		base.Start ();
		StartCoroutine(HandleShoot());
	}

	void Shoot() {
		if (thing.dead)
			return;
		GameObject newBullet = Instantiate (bullet, faceRight ? (transform.position + 30f * Vector3.right) : (transform.position + 30f * Vector3.left), Quaternion.identity);
		Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D> ();
		bulletBody.velocity = new Vector2(faceRight ? bulletSpeed : -bulletSpeed, 0f);

	}

	IEnumerator HandleShoot() {
		while (true) {
			yield return new WaitForSeconds (shootInterval);
			Shoot ();
		}

	}
}
