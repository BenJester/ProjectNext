using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderAdapter : MonoBehaviour {

	// Use this for initialization
	void Start () {

		
		GetComponent<BoxCollider2D>().size=new Vector3(GetComponent<SpriteRenderer>().bounds.size.x,GetComponent<SpriteRenderer>().bounds.size.y,0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
