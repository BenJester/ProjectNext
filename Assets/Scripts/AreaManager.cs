using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour {

	public int areaID;
	public bool checkited=false;

	

	void Start () {
		DontDestroyOnLoad(gameObject);
	}

	public void HandleRestart() {
		
	}

	void Update () {

	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag=="player")
		{
			checkited=true;
			print("enter!");
			if(CheckPointTotalManager.instance)
			CheckPointTotalManager.instance.SetPlayerPos (transform.position);
		}
	}


}
