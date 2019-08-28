using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotate : MonoBehaviour {

	// Use this for initialization
	public float rotateSpeed=15;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.forward*rotateSpeed);
	}
}
