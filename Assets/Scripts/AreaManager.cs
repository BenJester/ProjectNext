using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AreaManager : MonoBehaviour {

	//public int areaID;
	public bool checkited=false;
    public GameObject particle;
    public UnityEvent checkedAfterEvent;
	

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


            checkedAfterEvent.Invoke();
            checkited =true;
			//print("enter!");
			if(CheckPointTotalManager.instance)
			CheckPointTotalManager.instance.SetPlayerPos (transform.position);
		}
	}


}
