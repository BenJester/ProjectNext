using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public string levelName;
	Goal goal;

	void Start() {
		goal = GameObject.FindWithTag ("goal").GetComponent<Goal> ();
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag("bullet")) {
			col.GetComponent<Bullet> ().Deactivate ();
			StartCoroutine(goal.NextLevel (0.3f, levelName));
		}
	}

}
