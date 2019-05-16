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

	private List<Rigidbody2D> enemyBody;
	private List<Rigidbody2D> objBody;
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
}
