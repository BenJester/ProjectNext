using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUnlock : MonoBehaviour {

	public bool bulletTimeOn;
	public bool dashOn;
	public bool slowBulletOn;
	SkillManager skillManager;

	void Start() {
		transform.GetChild (0).gameObject.SetActive (false);
		skillManager = GameObject.FindWithTag ("WorldManager").GetComponent<SkillManager> ();
	}

	void OnTriggerEnter2D(Collider2D col){
		if (bulletTimeOn)
			skillManager.bulletTimeOn = true;
		if (dashOn)
			skillManager.dashOn = true;
		if (slowBulletOn)
			skillManager.slowBulletOn = true;
		skillManager.Init ();
		transform.GetChild (0).gameObject.SetActive (true);
	}
}
