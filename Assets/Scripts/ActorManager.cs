using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActorManager : MonoBehaviour {

	private static ActorManager _instance;
	public static ActorManager Instance { 
		get { 	
			return _instance; 
		}
	}

	public static List<GameObject> enemies;
	public static List<GameObject> obj;
	public static List<GameObject> bullets;

	void Awake () {
		if (ActorManager._instance == null) {
			ActorManager._instance = this;
			_instance.Init ();
		} else {
			if (ActorManager._instance != this) {
				throw new InvalidOperationException ("Cannot have two instances of a Singleton");
			}
		}
	}

	private void Init () {
		enemies = new List<GameObject> ();
		obj = new List<GameObject> ();
		bullets = new List<GameObject> ();
	}


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
