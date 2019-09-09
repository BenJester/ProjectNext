using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

	// Use this for initialization

	public GameObject[] GOs;
	
	public float time;
	public enum eventType{
		appear=0,
	}
	void Start () {
		foreach (var item in GOs)
		{
			item.SetActive(false);
		}
		StartCoroutine(appear(time));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator appear(float time){
		yield return new WaitForSeconds(time);
		 foreach(var go in GOs){
			 go.SetActive(true);
		 }
		
	}
}
