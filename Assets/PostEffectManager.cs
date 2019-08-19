using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffectManager : MonoBehaviour {


	public static PostEffectManager instance;
	// Use this for initialization
	public GameObject effect;

	private void Awake() {
		if(instance==null) instance=this;
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void Blink(float time){
		StartCoroutine(BlinkEffect(time));
	}
	IEnumerator BlinkEffect(float time){
		effect.SetActive(true);
		print("effectOn");
		yield return new WaitForSeconds(time);
		effect.SetActive(false);
		
		print("effectOff");
	}
}
