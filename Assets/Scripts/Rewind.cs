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

//	public int bluePillCount;
//	public int orangePillCount;
//
//	public bool blueFacingRight;
//	public bool orangeFacingRight;

//	public List<bool> bluePillState;
//	public List<bool> orangePillState;
//
//	public List<bool> trapBlockState;
//
//	public bool blueTeleporting;
//	public bool orangeTeleporting;

//	public List<GameObject> blocks;

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

	private static Rewind _instance;
	public static Rewind Instance { 
		get { 	
			return _instance; 
		}
	}

	public GameObject player;

	public int maxLength = 100;

	public bool isReverting;
	public List<GameState> states;

	private GameObject[] enemies;
	private GameObject[] obj;
	private GameObject[] bullets;

	private List<Rigidbody2D> enemyBody;
	private List<Rigidbody2D> objBody;
	private List<Rigidbody2D> bulletBody;
	private Rigidbody2D playerBody;

	public SpriteRenderer arrow;
	public SpriteRenderer blackBg;

	void Awake () {
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

		states = new List<GameState> ();

		enemyBody = new List<Rigidbody2D> ();
		objBody = new List<Rigidbody2D> ();

		playerBody = player.GetComponent<Rigidbody2D> ();

		foreach (GameObject enemy in ActorManager.enemies) {
			enemyBody.Add (enemy.GetComponent<Rigidbody2D> ());
		}
		foreach (GameObject obj in ActorManager.obj) {
			objBody.Add (obj.GetComponent<Rigidbody2D> ());
		}
		foreach (GameObject bullet in ActorManager.bullets) {
			objBody.Add (bullet.GetComponent<Rigidbody2D> ());
		}
	}

	void Update () {
		if (isReverting) {
			blackBg.color = new Color (255f, 255f, 255f, Mathf.Clamp(blackBg.color.a + 0.02f, 0f, 0.2f));
			arrow.color = new Color (255f, 255f, 255f, Mathf.Clamp(arrow.color.a + 0.07f, 0f, 0.7f));
		} else {
			blackBg.color = new Color (255f, 255f, 255f, Mathf.Clamp(blackBg.color.a - 0.02f, 0f, 0.2f));
			arrow.color = new Color (255f, 255f, 255f, Mathf.Clamp(arrow.color.a - 0.07f, 0f, 0.7f));
		}
	}

	bool shouldRecord() {
		// if nothing is moving, should not record
		if (states.Count == 0)
			return true;
		
		GameState last = states[states.Count - 1];

		for (int i = 0; i < ActorManager.obj.Count; i ++) {
			if (obj [i].transform.position != last.objPos [i])
				return true;
		}

		for (int i = 0; i < ActorManager.enemies.Count; i ++) {
			if (enemies [i].transform.position != last.enemyPos [i])
				return true;
		}
		for (int i = 0; i < ActorManager.bullets.Count; i ++) {
			if (enemies [i].transform.position != last.enemyPos [i])
				return true;
		}
		if (player.transform.position != last.playerPos) {
			return true;
		}

		return false;
	}

//	public void Record() {
//		if (!active)
//			return;
//
//		if (!shouldRecord ())
//			return;
//
//		GameState state = new GameState ();
//		state.playerPos = player.transform.position;
//		state.playerV = playerBody.velocity;
//		for (int i = 0; i < obj.Length; i ++) {
//			if (obj [i].transform.position != last.objPos [i])
//				return true;
//		}
//	}
}
