using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActorManager : MonoBehaviour {

    private SpriteRenderer black;

	//private static ActorManager _instance;
	//public static ActorManager Instance { 
	//	get { 	
	//		return _instance; 
	//	}
	//}

	//public static List<GameObject> enemies;
	//public static List<GameObject> obj;
	//public static List<GameObject> bullets;

	void Awake () {
        black = GameObject.FindGameObjectWithTag("black").GetComponent<SpriteRenderer>();
		//if (ActorManager._instance == null) {
		//	ActorManager._instance = this;
		//	_instance.Init ();
		//} else {
		//	if (ActorManager._instance != this) {
		//		throw new InvalidOperationException ("Cannot have two instances of a Singleton");
		//	}
		//}
	}

	private void Init () {
		//enemies = new List<GameObject> ();
		//obj = new List<GameObject> ();
		//bullets = new List<GameObject> ();
	}


	void Start () {
        black.color = Color.white;

        StartCoroutine(FadeOutBlack(0.15f));
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    IEnumerator FadeOutBlack(float duration)
    {
        while (black.color.a > 0f)
        {
            black.color = new Color(black.color.r, black.color.g, black.color.b, Mathf.Clamp01(black.color.a - Time.deltaTime / duration));
            yield return new WaitForSeconds(0.02f);
        }
    }
}
