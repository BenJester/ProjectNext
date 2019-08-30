using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Level_Bound : MonoBehaviour {

	
	[Header("用于拜访在全关卡，如果玩家走到边界外，进入下一个关卡场景")]
	public int nextLevel;
	private WorldLevelManager wlm;

	private void Awake() {
		wlm = GameObject.FindGameObjectWithTag("WorldManager").GetComponent<WorldLevelManager>();
	}
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		SceneManager.LoadScene(nextLevel);
	}
}
