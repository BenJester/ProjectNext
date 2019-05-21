using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour {

    public GameObject textEnd;
	public bool active;
	public float enemyCount;
	public List<PhysicalButton> buttonList;
	public List<Thing> hostageList;

	public Sprite activeSprite;
	public Sprite inactiveSprite;
	public SpriteRenderer black;

	SpriteRenderer sr;
	bool won;
	 
	void Awake() {
		buttonList = new List<PhysicalButton> ();

	}
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
        textEnd = GameObject.FindGameObjectWithTag("leveltitle");
		black = GameObject.FindWithTag ("black").GetComponent<SpriteRenderer> ();
        textEnd.SetActive(false);

    }
	
	void Update () {
		if (enemyCount <= 0 && checkButtons() && checkHostages()) {
            active = true;
            sr.sprite = activeSprite;
		} else {
			active = false;
			sr.sprite = inactiveSprite;
		}
	}

	bool checkButtons() {
		for (int i = 0; i < buttonList.Count; i++) {
			if (buttonList [i].state != ClickState.IsClick)
				return false;
		}
		return true;
	}

	bool checkHostages() {
		for (int i = 0; i < buttonList.Count; i++) {
			Debug.Log (hostageList [i].dead);
			if (hostageList [i].dead) {
				return false;

			}
		}
		return true;
	}

	void OnTriggerStay2D(Collider2D col) {
		if (col.CompareTag("player") && active && !won) {
			won = true;
//			Rewind.Instance.watching = true;
			Rewind.Instance.active = false;
			StartCoroutine (NextLevel (0.7f));

		}
	}

	IEnumerator NextLevel(float duration) {
		while (Rewind.Instance.watching) {
			yield return new WaitForEndOfFrame();
		}
		while (black.color.a < 1f) {
            textEnd.SetActive(true);
            textEnd.transform.GetChild(0).gameObject.SetActive(true);

            black.color = new Color (black.color.r, black.color.g, black.color.b, Mathf.Clamp01 (black.color.a + Time.deltaTime / duration));
			yield return new WaitForSeconds (0.02f);
		}

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

	}
	
}
