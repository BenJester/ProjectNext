using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour {

	public int areaID;
	int playerArea;

	GameObject player;
	public GameObject respawnPoint;

	void Start () {
		player = GameObject.FindWithTag ("player");
		DontDestroyOnLoad (player);
		player.transform.position = respawnPoint.transform.position;
	}

	public void HandleRestart() {
		
	}

	void Update () {

	}
}
