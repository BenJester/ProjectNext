using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceManager : MonoBehaviour {

	public static ForceManager instance;


	private void Awake() {
		if(instance==null) instance=this;
	}
	// Use this for initialization

	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.T)){
			AddGmaeObjectForce(GetComponent<Rigidbody2D>(),100000000,new Vector2(-1,1));
		}
	}

	public void AddGmaeObjectForce (Rigidbody2D target, float force, Vector2 dir) {
		target.AddForce(dir.normalized*force);
	}

}

