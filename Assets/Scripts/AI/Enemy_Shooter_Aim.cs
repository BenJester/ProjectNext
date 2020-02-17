using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shooter_Aim : Enemy {

    // Use this for initialization

    [Space]
    [Header("静态敌人————会主动瞄准玩家进行射击")]


    public bool aimNoCollider=false;
	public float distance;
	public GameObject bullet;
	private Vector2 direction;
	public float bulletSpeed;
	public float shootInterval = 1f;
	public float animationPreload = 0.1f;
	public float bulletInstanceDistance = 50f;
	private Animator animator;
	public Transform player;
	public bool isInSight = false;
	public LineRenderer lr;
   

	private void Awake () {
		animator = GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag ("player").GetComponent<Transform> ();
		lr = GetComponent<LineRenderer> ();
	}
	void Start () {
		base.Start ();
		StartCoroutine (HandleShoot ());
		//transform.rotation = Quaternion.Euler (0, 0, AngleBetween (direction, Vector2.left));
	}

	// Update is called once per frame
	void Update () {

		if (thing.dead) {
			lr.enabled = false;
			return;
		}
		direction = (player.position - transform.position).normalized;
		//在视线中
		isInSight = CheckPlayerInSight ();

		//画线；
		if (isInSight) {
			lr.enabled = true;
			Debug.DrawLine (transform.position, transform.position + (Vector3) direction * distance, Color.red, 0.1f);
			lr.SetPosition (0, transform.position);
			lr.SetPosition (1, (player.position - transform.position).normalized * distance + transform.position);
		} else lr.enabled = false;

	}

	IEnumerator HandleShoot () {

		while (true) {
			if (isInSight) {

				yield return new WaitForSeconds (shootInterval - animationPreload);
				animator.CrossFade ("Enemy_Shooter_Shot", 0.001f);
                exclamation.SetActive(true);
                yield return new WaitForSeconds (animationPreload);
				Shoot ();
                exclamation.SetActive(false);
            } else {
				yield return null;
			}
		}

	}

	void Shoot () {
		if (thing.dead)
			return;
		GameObject newBullet = Instantiate (bullet, transform.position + bulletInstanceDistance * (Vector3) direction, Quaternion.identity);
		Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D> ();
		bulletBody.velocity = direction * bulletSpeed;

	}

	public static float AngleBetween (Vector2 vectorA, Vector2 vectorB) {
		float angle = Vector2.Angle (vectorA, vectorB);
		Vector3 cross = Vector3.Cross (vectorA, vectorB);

		if (cross.z > 0) {
			angle = 360 - angle;
		}

		return angle;
	}

	public bool CheckPlayerInSight () {


        if (aimNoCollider)
        {
            if(Vector2.Distance(transform.position, player.position) <= distance) return true;
            else return false;
        }

		RaycastHit2D[] hits = Physics2D.RaycastAll (transform.position, (player.position - transform.position).normalized, distance, (1 << 10) | (1 << 8) | (1 << 9));
		RaycastHit2D hitNear;
		if (hits.Length >= 2) {
			hitNear = hits[1];
			if (hitNear.collider.tag == "player") return true;
			else return false;
		} else return false;
	}
	private void OnDrawGizmos () {
		if (player != null) Gizmos.DrawLine (transform.position, (player.position - transform.position).normalized * distance + transform.position);
	}

}