using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy {

	// Use this for initialization
	
	public GameObject bullet;
    public GameObject laser;
	public float bulletSpeed;

    

	[Header("不打开就不会发射子弹")]
	public bool BeSetShoot = true;
	public float shootInterval = 1f;
	public float animationPreload=0.1f;
	private Animator animator;

	int count;


	private void Awake() {
		animator= GetComponent<Animator>();
	}
	void Start () {
		base.Start ();
		StartCoroutine(HandleShoot());
		if (faceRight)
		{
			GetComponent<SpriteRenderer>().flipX=true;

		}
	}
    public float yBulletSpeedOffset;

	void Shoot() {
		if (thing.dead)
			return;
		GameObject newBullet = Instantiate (bullet, faceRight ? (transform.position + 60f * Vector3.right) : (transform.position + 60f * Vector3.left), Quaternion.identity);
		Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D> ();
		bulletBody.velocity = new Vector2(faceRight ? bulletSpeed : -bulletSpeed, yBulletSpeedOffset);
        if (yBulletSpeedOffset != 0f)
            newBullet.GetComponent<TrailRenderer>().enabled = false;
	}

    private void Update()
    {
        GetComponent<SpriteRenderer>().flipX = faceRight;
    }

    IEnumerator HandleShoot() {
		while (BeSetShoot) {
			yield return new WaitForSeconds (shootInterval-animationPreload);
			animator.CrossFade("Enemy_Shooter_Shot",0.001f);
			yield return new WaitForSeconds(animationPreload);
			Shoot ();
		}

	}

	public void PowerfulShoot(){
		StartCoroutine(HandlePowerfulShoot());

	}


	IEnumerator HandlePowerfulShoot(){
		
		yield return new WaitForSeconds(0.05f);
		GameObject newBullet = Instantiate (bullet, faceRight ? (transform.position + 60f * Vector3.right) : (transform.position + 60f * Vector3.left), Quaternion.identity);
		Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D> ();
		bulletBody.velocity = new Vector2(faceRight ? bulletSpeed*3 : -bulletSpeed*3, 0f);
	}
}
