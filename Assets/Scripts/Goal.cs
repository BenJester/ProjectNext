using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour {

	public bool active;
	public float enemyCount;
	public Sprite activeSprite;
	public Sprite inactiveSprite;
	public SpriteRenderer black;

	SpriteRenderer sr;
	bool won;

	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		black = GameObject.FindWithTag ("black").GetComponent<SpriteRenderer> ();
		black.color = Color.white;
		StartCoroutine (FadeOutBlack (0.15f));
	}
	
	void Update () {
		if (enemyCount <= 0) {
            active = true;
            sr.sprite = activeSprite;
		} else {
			active = false;
			sr.sprite = inactiveSprite;
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag("player") && active && !won) {
			won = true;
			StartCoroutine (NextLevel (0.7f));
		}
	}

	IEnumerator NextLevel(float duration) {
		while (black.color.a < 1f) {
			black.color = new Color (black.color.r, black.color.g, black.color.b, Mathf.Clamp01 (black.color.a + Time.deltaTime / duration));
			yield return new WaitForSeconds (0.02f);
		}

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

	}
	IEnumerator FadeOutBlack(float duration) {
		while (black.color.a > 0f) {
			black.color = new Color (black.color.r, black.color.g, black.color.b, Mathf.Clamp01 (black.color.a - Time.deltaTime / duration));
			yield return new WaitForSeconds (0.02f);
		}
	}
}
