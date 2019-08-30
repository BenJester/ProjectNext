using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitSkill : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.Find ("WorldManager").GetComponent<SkillManager> ().Init();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
