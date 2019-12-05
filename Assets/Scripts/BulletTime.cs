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
    public enum BulletTimePriority
    {
        BulletTimePriority_None = 0,
        BulletTimePriority_Low = 1,
        BulletTimePriority_High = 2,
    }

    private BulletTimePriority m_curPriority;
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
				if (Input.GetKeyDown (KeyCode.LeftShift)) {
                    ActiveBulletTime(!bulletTimeActive, BulletTimePriority.BulletTimePriority_High);
				}
				break;

			case inputMode.HoldToIn:
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    ActiveBulletTime(true, BulletTimePriority.BulletTimePriority_High);
                }
                else
                {
                    ActiveBulletTime(false, BulletTimePriority.BulletTimePriority_High);
                }
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
    public void ActiveBulletTime(bool bActive, BulletTimePriority ePriority)
    {
        if(m_curPriority <= ePriority)
        {
            bulletTimeActive = bActive;
            if(bActive == false)
            {
                m_curPriority = BulletTimePriority.BulletTimePriority_None;
            }
            else
            {
                m_curPriority = ePriority;
            }
        }
    }
}