using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

	public GameObject target;
	GameObject player;

	void Start () {
		player = GameObject.FindWithTag ("player");
	}
	
	private void OnTriggerEnter2D(Collider2D col) {
		if (!target)
			return;
		player.transform.position = target.transform.position;
	}
}
