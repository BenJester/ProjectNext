using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Dasher_Aim :  Enemy {

	[Space]
	[Header ("动态敌人————会主动瞄准玩家进行冲刺")]

	//TODO:还没有加入动画
	public PlayerState state;
	public int aimFrame = 120;
	public int lockFrame = 30;
	public int dashFrame = 30;
	public float dashSpeed=20;
	public int recoverFrame = 120;

	public enum PlayerState {
		idle = 0,
		aim = 1,
		lockOn = 2,
		dash = 3,
		recover = 4,

		isDead = 5,
	}

	//Extra
	[System.NonSerialized] public int[] stateActiveFrames = new int[5] { 0, 0, 0, 0, 0 };
	[System.NonSerialized] public int[] stateInactiveFrames = new int[5] { 0, 0, 0, 0, 0 };

	private void TimeFixedUpdate () {
		for (int i = 0; i < stateInactiveFrames.Length; i++) {
			stateInactiveFrames[i] = ((int) state == i) ? 0 : stateInactiveFrames[i] + 1;
		}

		for (int i = 0; i < stateActiveFrames.Length; i++) {
			stateActiveFrames[i] = ((int) state == i) ? stateActiveFrames[i] + 1 : 0;
		}
	}

	private void StateFixedUpdate () {
		if (state == PlayerState.idle) {
			if (health == 0) {
				state = PlayerState.isDead;
			} else if (CheckPlayerInSight ()) state = PlayerState.aim;
		} else if (state == PlayerState.aim) {
			if (stateActiveFrames[(int) PlayerState.aim] > aimFrame) {
				state = PlayerState.lockOn;
			}
		} else if (state == PlayerState.lockOn) {
			if (stateActiveFrames[(int) PlayerState.lockOn] > lockFrame) {
				state = PlayerState.dash;
			}
		} else if (state == PlayerState.dash) {
			if (stateActiveFrames[(int) PlayerState.dash] > dashFrame) {
				state = PlayerState.recover;
			}
		} else if (state == PlayerState.recover) {
			if (stateActiveFrames[(int) PlayerState.recover] > recoverFrame) {
				state = PlayerState.idle;
			}
		}
	}

	public float distance=1000;
	private Vector2 direction;
	
	private Animator animator;
	public Transform player;
	
	public LineRenderer lr;

	private void Awake () {
		animator = GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag ("player").GetComponent<Transform> ();
		lr = GetComponent<LineRenderer> ();
	}
	void Start () {
		base.Start ();
		//StartCoroutine (HandleShoot ());
		//transform.rotation = Quaternion.Euler (0, 0, AngleBetween (direction, Vector2.left));
	}

	// Update is called once per frame
	void Update () {
		if (thing.dead) {
			lr.enabled = false;
			return;
		}
		TimeFixedUpdate ();
		StateFixedUpdate ();

		switch (state) {
			case PlayerState.idle:
				if (stateActiveFrames[(int) PlayerState.idle] == 0) lr.enabled = false;
				break;
			case PlayerState.aim:
				if (stateActiveFrames[(int) PlayerState.aim] == 0) lr.enabled = true;
				//Debug.DrawLine (transform.position, transform.position + (Vector3) direction * distance, Color.red, 0.1f);
				lr.SetPosition (0, transform.position);
				lr.SetPosition (1, (Vector3) direction * distance + transform.position);
				lr.startColor = Color.green;
				lr.endColor = Color.green;
				//刷新方向
				direction = (player.position - transform.position).normalized;
				break;

			case PlayerState.lockOn:
				lr.SetPosition (0, transform.position);
				lr.SetPosition (1, (Vector3) direction * distance + transform.position);
				lr.startColor = Color.yellow;
				lr.endColor = Color.yellow;
				break;

		case PlayerState.dash:
			if (stateActiveFrames [(int)PlayerState.recover] == 0) {
				EnemyDash ();
				lr.enabled = false;
			} 
				
				break;

			case PlayerState.recover:
				if (stateActiveFrames[(int) PlayerState.recover] == 0) lr.enabled = false;
				break;
			case PlayerState.isDead:
				return;

		}

	}

	private void FixedUpdate () {

	}

	// IEnumerator HandleShoot () {

	// 	while (true) {
	// 		if (isInSight) {

	// 			yield return new WaitForSeconds (shootInterval - animationPreload);
	// 			animator.CrossFade ("Enemy_Shooter_Shot", 0.001f);
	// 			yield return new WaitForSeconds (animationPreload);
	// 			//Shoot ();
	// 		} else {
	// 			yield return null;
	// 		}
	// 	}

	// }

	void EnemyDash () {
		if (thing.dead)
			return;
		

		//TODO:这部分不知道怎么移动
		
		//transform.GetComponent<Rigidbody2D>().AddForce(transform.position+(Vector3)direction*dashSpeed);
		transform.GetComponent<Rigidbody2D>().velocity = (Vector2) (direction * dashSpeed);
		RaycastHit2D[] hits = Physics2D.RaycastAll (transform.position, direction, 50, (1 << 10) | (1 << 8) | (1 << 9));
		RaycastHit2D hitNear;
		if (hits.Length >= 2) {
			hitNear = hits[1];
			if (hitNear.collider.tag == "player") hitNear.collider.GetComponent<PlayerControl> ().Die ();
		}

	}

	public bool CheckPlayerInSight () {
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

	public static float AngleBetween (Vector2 vectorA, Vector2 vectorB) {
		float angle = Vector2.Angle (vectorA, vectorB);
		Vector3 cross = Vector3.Cross (vectorA, vectorB);

		if (cross.z > 0) {
			angle = 360 - angle;
		}

		return angle;
	}
}
