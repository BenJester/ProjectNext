using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BulletTime : MonoBehaviour {

	private float startDeltaTime;
	private float targetDeltaTime;
	private float targetTimeScale;

	public  PostProcessVolume bulletTimePostEffect;
	public inputMode mode = inputMode.ClickToInOut;
	public bool bulletTimeActive = false;
	public enum inputMode {
		ClickToInOut = 0,
		HoldToIn = 1,
	}

	// Use this for initialization
	private void Awake () {
		startDeltaTime = Time.fixedDeltaTime;
		targetDeltaTime = startDeltaTime;
		targetTimeScale = 1f;
		bulletTimePostEffect=GameObject.FindGameObjectWithTag("BulletTimeEffect").GetComponent<PostProcessVolume>();
	}
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		switch (mode) {
			case inputMode.ClickToInOut:
				if (Input.GetKeyDown (KeyCode.Space)) {
					bulletTimeActive = !bulletTimeActive;
				}
				break;

			case inputMode.HoldToIn:
				if (Input.GetKey (KeyCode.Space)) {
					bulletTimeActive = true;
				} else bulletTimeActive = false;
				break;

		}

		if (bulletTimeActive) {
			Time.timeScale = 0.1f;
			targetTimeScale = 0.1f;
			Time.fixedDeltaTime = startDeltaTime * 0.1f;
			targetDeltaTime = startDeltaTime * 0.1f;
			bulletTimePostEffect.enabled=true;
		} else {
			targetTimeScale = 1f;
			targetDeltaTime = startDeltaTime;
			bulletTimePostEffect.enabled=false;
		}

	}
}