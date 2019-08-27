using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTime : MonoBehaviour {

	private float startDeltaTime;
	private float targetDeltaTime;
	private float targetTimeScale;

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
	}
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		switch (mode) {
			case inputMode.ClickToInOut:
				if (Input.GetKeyDown (KeyCode.LeftShift)) {
					bulletTimeActive = !bulletTimeActive;
				}
				break;

			case inputMode.HoldToIn:
				if (Input.GetKey (KeyCode.LeftShift)) {
					bulletTimeActive = true;
				} else bulletTimeActive = false;
				break;

		}

		if (bulletTimeActive) {
			Time.timeScale = 0.1f;
			targetTimeScale = 0.1f;
			Time.fixedDeltaTime = startDeltaTime * 0.1f;
			targetDeltaTime = startDeltaTime * 0.1f;
		} else {
			targetTimeScale = 1f;
			targetDeltaTime = startDeltaTime;
		}

	}
}