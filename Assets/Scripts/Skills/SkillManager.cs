using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour {

	public bool bulletTimeOn;
	public bool dashOn;
	public bool slowBulletOn;

	GameObject player;

	void Start () {
		
	}

	public void Init() {
		player = GameObject.FindWithTag ("player");
		if (bulletTimeOn)
			player.GetComponent<BulletTime> ().enabled = true;
		else
			player.GetComponent<BulletTime> ().enabled = false;
		
		if (dashOn)
			player.GetComponent<Dash> ().active = true;
		else 
			player.GetComponent<Dash> ().active = false;
		
		if (slowBulletOn)
			player.GetComponent<PlayerControl1> ().maxBulletSpeed = 400;
		else
			player.GetComponent<PlayerControl1> ().maxBulletSpeed = 2000;
	}
	// Update is called once per frame
	void Update () {
		
	}
}
