using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour {

	public bool active;
	public float enemyCount;
	public Sprite activeSprite;
	public Sprite inactiveSprite;

	SpriteRenderer sr;

	void Start () {
		sr = GetComponent<SpriteRenderer> ();
	}
	
	void Update () {
		if (enemyCount <= 0) {
			active = true;
			sr.sprite = activeSprite;
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag("player") && active) {
			LevelClear ();
		}
	}

	void LevelClear() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
