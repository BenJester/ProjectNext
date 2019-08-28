using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayAnimation : MonoBehaviour {

	// Use this for initialization

	
	private Vector3 now;
	private Vector3 temp;
	void Start () {
		
	}
	
	// Update is called once per frame
	private void Update()
	{
		now = transform.position;

		if(temp != now){
			GetComponent<Animator>().CrossFade("Focus",0.001f);
			Debug.Log("MOVE");
		}
		temp=transform.position;
	}

}
