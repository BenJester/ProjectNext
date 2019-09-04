using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shooter_Rotate : Enemy {





	[Space]
	[Header("静态敌人————可以调整子弹射击方向的敌人")]
	[Header("拖入子弹并且设置子弹射击的方向")]
	//TODO:还需要设置一个动画，以及旋转
	public GameObject bullet;
	
	public Vector2 direction;
	public float bulletSpeed;
	public float shootInterval = 1f;
	public float animationPreload = 0.1f;
	public float bulletInstanceDistance = 50f;
	private Animator animator;
	
	int count;

	private void Awake () {
		animator = GetComponent<Animator> ();
	}
	void Start () {
		base.Start ();
		StartCoroutine (HandleShoot ());
		direction=direction.normalized;

		transform.rotation =  Quaternion.Euler(0,0,AngleBetween(direction,Vector2.left));
	}

	// Update is called once per frame
	void Update () {

	}

	IEnumerator HandleShoot () {
		while (true) {
			yield return new WaitForSeconds (shootInterval - animationPreload);
			animator.CrossFade ("Enemy_Shooter_Shot", 0.001f);
			yield return new WaitForSeconds (animationPreload);
			Shoot ();
		}

	}


	void Shoot() {
		if (thing.dead)
			return;
		GameObject newBullet = Instantiate (bullet, transform.position + bulletInstanceDistance * (Vector3)direction,Quaternion.identity);
		Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D> ();
		bulletBody.velocity = direction*bulletSpeed;

	}

	public static float AngleBetween (Vector2 vectorA, Vector2 vectorB) {
		float angle = Vector2.Angle (vectorA, vectorB);
		Vector3 cross = Vector3.Cross (vectorA, vectorB);

		if (cross.z > 0) {
			angle = 360 - angle;
		}

		return angle;
	}
}