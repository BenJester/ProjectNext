using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour {

	//public int areaID;
	public bool checkited=false;
    public GameObject particle;
	

	void Start () {
		//DontDestroyOnLoad(gameObject);
	}

	public void HandleRestart() {
		
	}

	void Update () {

	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag=="player")
		{
            if (!checkited && particle != null) {
                GameObject part1 = Instantiate(particle, transform.position, Quaternion.identity);
                Destroy(part1,1.5f);
            }
                
			checkited=true;
			//print("enter!");
			if(CheckPointTotalManager.instance)
			CheckPointTotalManager.instance.SetPlayerPos (transform.position);
		}
	}


}
