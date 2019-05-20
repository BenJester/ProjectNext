using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameState {

	public Vector3 playerPos;
	public Vector2 playerV;

	public List<bool> enemyState;
	public List<Vector3> enemyPos;
	public List<Vector2> enemyV;

	public List<Vector3> objPos;
	public List<Vector2> objV;

	public List<bool> bulletState;
	public List<Vector3> bulletPos;
	public List<Vector2> bulletV;

	public float timeScale;
	public float fixedDeltaTime;

	public GameState () {
		
		enemyState = new List<bool> ();
		enemyPos = new List<Vector3> ();
		enemyV = new List<Vector2> ();

		objPos = new List<Vector3> ();
		objV = new List<Vector2> ();

		bulletState = new List<bool> ();
		bulletPos = new List<Vector3> ();
		bulletV = new List<Vector2> ();
	}
}

public class Rewind : MonoBehaviour {

	public bool active;
	public bool watching;

	int counter;

	private static Rewind _instance;
	public static Rewind Instance { 
		get { 	
			return _instance; 
		}
	}

	public GameObject player;

	public int maxLength = 500;

	public bool isReverting;
	public List<GameState> states;

	public List<GameObject> enemies;
	public List<GameObject> obj;
	public List<GameObject> bullets;

	public List<Rigidbody2D> enemyBody;
	public List<Rigidbody2D> objBody;
	public List<Rigidbody2D> bulletBody;

	private Rigidbody2D playerBody;

	public SpriteRenderer arrow;
	public SpriteRenderer blackBg;

	bool lateInit;

	int watchIndex = 0;

	void Awake () {
		player = GameObject.FindWithTag ("player");

		if (Rewind._instance == null) {
			Rewind._instance = this;
			_instance.Init ();
		} else {
			if (Rewind._instance != this) {
				throw new InvalidOperationException ("Cannot have two instances of a Singleton");
			}
		}
	}



	private void Init () {
		if (!active)
			return;

		enemies = new List<GameObject> ();
		obj = new List<GameObject> ();
		bullets = new List<GameObject> ();

		states = new List<GameState> ();

		enemyBody = new List<Rigidbody2D> ();
		objBody = new List<Rigidbody2D> ();
		bulletBody = new List<Rigidbody2D>();

		playerBody = player.GetComponent<Rigidbody2D> ();

	}
		

	void Update () {

		if (!lateInit) {
			lateInit = true;
			foreach (GameObject enemy in enemies) {
				enemyBody.Add (enemy.GetComponent<Rigidbody2D> ());
			}
			foreach (GameObject obj in obj) {
				objBody.Add (obj.GetComponent<Rigidbody2D> ());
			}
			foreach (GameObject bullet in bullets) {
				bulletBody.Add (bullet.GetComponent<Rigidbody2D> ());
			}
		}

		if (isReverting) {
			blackBg.color = new Color (255f, 255f, 255f, Mathf.Clamp(blackBg.color.a + 0.02f, 0f, 0.2f));
			arrow.color = new Color (255f, 255f, 255f, Mathf.Clamp(arrow.color.a + 0.07f, 0f, 0.7f));
		} else {
			blackBg.color = new Color (255f, 255f, 255f, Mathf.Clamp(blackBg.color.a - 0.02f, 0f, 0.2f));
			arrow.color = new Color (255f, 255f, 255f, Mathf.Clamp(arrow.color.a - 0.07f, 0f, 0.7f));
		}

		if (watching)
			Watch ();
	}

	bool shouldRecord() {
		// if nothing is moving, should not record
		if (states.Count == 0)
			return true;
		
		GameState last = states[states.Count - 1];

		for (int i = 0; i < obj.Count; i ++) {
			if (obj [i].transform.position != last.objPos [i])
				return true;
		}

		for (int i = 0; i < enemies.Count; i ++) {
			if (enemies [i].transform.position != last.enemyPos [i])
				return true;
		}
		if (bullets.Count != last.bulletPos.Count)
			return true;
		
		for (int i = 0; i < bullets.Count; i ++) {
			if (bullets [i].transform.position != last.bulletPos [i])
				return true;
		}
		if (player.transform.position != last.playerPos) {
			return true;
		}

		return false;
	}

	public void Record() {
		if (!active)
			return;

		if (!shouldRecord ())
			return;

		int threshold = Mathf.CeilToInt (1 / Time.timeScale);

		if (counter < threshold) {
			counter += 1;
			return;
		}
		counter = 1;

		GameState state = new GameState ();
		state.playerPos = player.transform.position;
		state.playerV = playerBody.velocity;

		state.timeScale = Time.timeScale;
		state.fixedDeltaTime = Time.fixedDeltaTime;

		for (int i = 0; i < enemies.Count; i++) {
			state.enemyPos.Add (enemies [i].transform.position);
			state.enemyV.Add (enemyBody [i].velocity);
			state.enemyState.Add (enemies [i].GetComponent<Thing> ().dead);
		}

		for (int i = 0; i < bullets.Count; i++) {
			state.bulletPos.Add (bullets [i].transform.position);
			state.bulletV.Add (bulletBody [i].velocity);
			state.bulletState.Add (bullets [i].GetComponent<Bullet> ().active);
		}

		for (int i = 0; i < obj.Count; i++) {
			state.objPos.Add (obj [i].transform.position);
			state.objV.Add (objBody [i].velocity);
		}
		states.Add (state);

		if (states.Count > maxLength) {
			states.RemoveAt (0);
		}
	}

	public void Watch() {
		if (states == null || watchIndex > states.Count - 1) {
			watching = false;
			return;
		}



		GameState curr = states [watchIndex];
		player.transform.position = curr.playerPos;
		playerBody.velocity = curr.playerV;

		for (int i = 0; i < enemies.Count; i++) {
			enemies [i].transform.position = curr.enemyPos[i];
			enemyBody [i].velocity = curr.enemyV[i];
			Thing thing = enemies [i].GetComponent<Thing> ();
			if (thing.dead != curr.enemyState [i]) {
				if (!curr.enemyState [i])
					thing.Revive ();
				else
					thing.Die();
			}
		}

		for (int i = 0; i < curr.bulletPos.Count; i++) {
			bullets [i].transform.position = curr.bulletPos[i];
			bulletBody [i].velocity = curr.bulletV[i];
			Bullet bul = bullets [i].GetComponent<Bullet> ();
			if (bul.active != curr.bulletState [i]) {
				if (curr.bulletState [i])
					bul.Activate();
				else
					bul.Deactivate();
			}

		}

		if (curr.bulletPos.Count < bullets.Count) {
			for (int i = curr.bulletPos.Count; i < bullets.Count; i++) {
				bullets [i].GetComponent<Bullet> ().Deactivate ();
			}
		}

		for (int i = 0; i < obj.Count; i++) {
			obj [i].transform.position = curr.objPos[i];
			objBody [i].velocity = curr.objV[i];
		}
		watchIndex += 1;
	}

	public void Revert() {
		if (!active)
			return;
		
		if (states.Count < 3)
			return;
		
		GameState last = states[states.Count - 1];

		//Time.timeScale = 1f / last.timeScale;
		//Time.fixedDeltaTime = 1f / last.timeScale * last.fixedDeltaTime;

		player.transform.position = last.playerPos;
		playerBody.velocity = last.playerV;

		for (int i = 0; i < enemies.Count; i++) {
			enemies [i].transform.position = last.enemyPos[i];
			enemyBody [i].velocity = last.enemyV[i];
			Thing thing = enemies [i].GetComponent<Thing> ();
			if (thing.dead != last.enemyState [i]) {
				if (!last.enemyState [i])
					thing.Revive ();
				else
					thing.Die();
			}
		}

		for (int i = 0; i < last.bulletPos.Count; i++) {
			bullets [i].transform.position = last.bulletPos[i];
			bulletBody [i].velocity = last.bulletV[i];
			Bullet bul = bullets [i].GetComponent<Bullet> ();
			if (bul.active != last.bulletState [i]) {
				if (last.bulletState [i])
					bul.Activate();
				else
					bul.Deactivate();
			}
			
		}

		if (last.bulletPos.Count < bullets.Count) {
			for (int i = last.bulletPos.Count; i < bullets.Count; i++) {
				bullets [i].GetComponent<Bullet> ().Deactivate ();
			}
		}

		for (int i = 0; i < obj.Count; i++) {
			obj [i].transform.position = last.objPos[i];
			objBody [i].velocity = last.objV[i];
		}

		states.RemoveAt (states.Count - 1);
	}
}
