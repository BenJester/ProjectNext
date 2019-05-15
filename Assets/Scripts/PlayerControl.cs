using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {

	public float speed;
	public float jumpSpeed;

	public GameObject bullet;
	public float minBulletSpeed;
	public float maxBulletSpeed;
	public float bulletChargeSpeed;
	public Transform groundCheckPoint1;
	public Transform groundCheckPoint2;

	public float groundCheckRadius;
	public LayerMask groundLayer;
	private bool isTouchingGround;

	public float bulletSpeed;
	public int startChargeFrame;
	float chargeFrame = 0;

	public LineRenderer lr;

	Rigidbody2D rb;


	void Awake()
	{
		lr = GetComponent<LineRenderer> ();

		lr.enabled = false;

	}

	void Start() {
		bulletSpeed = minBulletSpeed;
		rb = GetComponent<Rigidbody2D> ();
	}

	void Update()
	{
		
		float h = (Input.GetKey(KeyCode.D) ? 1 : 0) + (Input.GetKey(KeyCode.A) ? -1 : 0);
		rb.velocity = new Vector2(h * speed, rb.velocity.y);

		isTouchingGround = Physics2D.OverlapCircle (groundCheckPoint1.position, groundCheckRadius, groundLayer) || Physics2D.OverlapCircle (groundCheckPoint2.position, groundCheckRadius, groundLayer);
		if (Input.GetKeyDown (KeyCode.W) && isTouchingGround) {
			rb.velocity = new Vector2 (rb.velocity.x, jumpSpeed);
		}

		if (Input.GetMouseButton (0)) {
			lr.enabled = true;
			lr.SetPosition (0, transform.position);
			Vector2 mousePosition = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
			lr.SetPosition (1, transform.position+ (Vector3) mousePosition.normalized * 9999);
			 
 			Time.timeScale = 0.1f;
			Time.fixedDeltaTime*=0.02f;
			IncreaseBulletSpeed ();
		}
		if (Input.GetMouseButtonUp (0)) {
			Time.timeScale = 1f;
			Time.fixedDeltaTime *= 50;
			lr.enabled = false;
			Shoot ();
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

	}

	void IncreaseBulletSpeed() {
		if (chargeFrame > startChargeFrame)
			bulletSpeed = Mathf.Clamp (bulletSpeed + bulletChargeSpeed, minBulletSpeed, maxBulletSpeed);
		else
			chargeFrame += 1;
	}

	void Shoot() {
		
		Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		GameObject newBullet = Instantiate (bullet, transform.position, Quaternion.identity);
		Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D> ();
		bulletBody.velocity = (mouseWorldPos - (Vector2) transform.position).normalized * bulletSpeed;

		bulletSpeed = minBulletSpeed;
		chargeFrame = 0;

	}
}
