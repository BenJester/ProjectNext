using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {

	public bool active;

	public float speed;
	public float jumpSpeed;
	public float maxSpeed = 10000f;

	public GameObject bullet;
	public float minBulletSpeed;
	public float maxBulletSpeed;
	public float bulletChargeSpeed;
	public Transform groundCheckPoint1;
	public Transform groundCheckPoint2;
	public Transform groundCheckPoint3;

	public float groundCheckRadius;
	public LayerMask groundLayer;
	private bool isTouchingGround;

	public float bulletSpeed;
	public int startChargeFrame;
	float chargeFrame = 0;

	public LineRenderer lr;

	Rigidbody2D rb;

	public float startDeltaTime;
	public float targetDeltaTime;
	public float targetTimeScale;

    //
    private bool isGroundTemp;
    public GameObject landingParticle;

	public Vector3 originalScale;

    void Awake()
	{
		originalScale = transform.localScale;
		startDeltaTime = Time.fixedDeltaTime;
		targetDeltaTime = startDeltaTime;
		targetTimeScale = 1f;

		lr = GetComponent<LineRenderer> ();
		lr.enabled = false;
	}

	void Start() {
		bulletSpeed = minBulletSpeed;
		rb = GetComponent<Rigidbody2D> ();
	}

	void Update()
	{
		isTouchingGround = Physics2D.Raycast (groundCheckPoint1.position, Vector3.down, 5f, groundLayer) || Physics2D.Raycast (groundCheckPoint2.position, Vector3.down, 5f, groundLayer) || Physics2D.Raycast (groundCheckPoint3.position, Vector3.down, 5f, groundLayer);

        //暂时没有生效不知道为什么
        if (isTouchingGround!=isGroundTemp && isTouchingGround==true && landingParticle!=null)
        {
            Instantiate(landingParticle, transform);
            isGroundTemp = isTouchingGround;
        }
      

		HandleRewind ();

		if (Input.GetKeyDown(KeyCode.F1))
		{
			SceneManager.LoadScene(0);
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			Time.fixedDeltaTime = startDeltaTime;
			Time.timeScale = 1f;
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		if (!Rewind.Instance.isReverting) {
			Time.timeScale = Mathf.Clamp (Time.timeScale + ((targetTimeScale >= Time.timeScale) ? 0.04f : -0.04f), 0.1f, 1f);
			Time.fixedDeltaTime = Mathf.Clamp (Time.fixedDeltaTime + ((targetDeltaTime >= Time.fixedDeltaTime) ? 0.04f * startDeltaTime : -0.04f * startDeltaTime), 0.1f * startDeltaTime, startDeltaTime);
		}

		if (!active)
			return;

		float h = (Input.GetKey(KeyCode.D) ? 1 : 0) + (Input.GetKey(KeyCode.A) ? -1 : 0);

		if (Mathf.Abs(rb.velocity.x) <= speed) {
			rb.velocity = new Vector2 (h * speed, Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed));
		} else {
			rb.velocity = new Vector2 (h * rb.velocity.x < 0 ? rb.velocity.x + 6f * h : rb.velocity.x, Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed));
		}

		if ((Input.GetKeyDown (KeyCode.W)||Input.GetKeyDown(KeyCode.Space)) && isTouchingGround) {
			rb.velocity = new Vector2 (rb.velocity.x, jumpSpeed);
		}

		if (Input.GetMouseButton (0)) {
			lr.enabled = true;
			HandleLineRenderer ();
			 
 			Time.timeScale = 0.1f;
			targetTimeScale = 0.1f;
			Time.fixedDeltaTime = startDeltaTime * 0.1f;
			targetDeltaTime = startDeltaTime * 0.1f;
			IncreaseBulletSpeed ();
		}
		if (Input.GetMouseButtonUp (0)) {
//			targetTimeScale = 1f;
//			targetDeltaTime = startDeltaTime;
			StartCoroutine(RestoreTimeScale(0.035f));
			lr.enabled = false;
			Shoot ();
		}

        
	}

	void FixedUpdate() {
		
	}

	void HandleLineRenderer() {
		lr.SetPosition (0, transform.position);
		Vector2 mousePosition = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
		lr.SetPosition (1, transform.position + (Vector3) mousePosition.normalized * 9999);
	}

	IEnumerator RestoreTimeScale(float duration) {
		yield return new WaitForSeconds (duration);
		if (!Input.GetMouseButton (0)) {
			targetTimeScale = 1f;
			targetDeltaTime = startDeltaTime;
		}
			
	}

	void HandleRewind() {
		if (Rewind.Instance != null) {
			if (Input.GetKey(KeyCode.Q)) {

				Rewind.Instance.isReverting = true;
			} else {
				Rewind.Instance.Record ();
				Rewind.Instance.isReverting = false;
			}

			if (!Rewind.Instance.isReverting) { //TODO: if no char or mushroom is moving, don't record



			} else {
				if (Rewind.Instance.states.Count == 0) {
					return;
				}
				Rewind.Instance.Revert ();
			}
		}
	}

	void IncreaseBulletSpeed() {
		if (chargeFrame > startChargeFrame)
			bulletSpeed = Mathf.Clamp (bulletSpeed + bulletChargeSpeed, minBulletSpeed, maxBulletSpeed);
		else
			chargeFrame += 1;
	}

	void Shoot() {
		Debug.Log (Input.mousePosition);
		Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		GameObject newBullet = Instantiate (bullet, transform.position + ((Vector3) mouseWorldPos - transform.position).normalized * 30f, Quaternion.identity);
		Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D> ();
		bulletBody.velocity = (mouseWorldPos - (Vector2) transform.position).normalized * bulletSpeed;

		bulletSpeed = minBulletSpeed;
		chargeFrame = 0;

	}
		
	public void Die() {
		active = false;
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Static;
		//transform.localScale = Vector3.zero;

	}
		
	public void Revive() {
		active = true;
		GetComponent<BoxCollider2D>().enabled = true;
		GetComponent<SpriteRenderer>().enabled = true;
		GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic;
		//transform.localScale = originalScale;
	}
}
