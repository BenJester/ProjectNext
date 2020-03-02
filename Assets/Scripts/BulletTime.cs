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

    private float m_fCurDelayActiveTime;
    private float m_fDelayActiveTime;
    private bool m_bDelayActive;

    private bool m_bCustomizeTime;
    private float m_fFixedDeltaTime;
    private float m_fTimeScale;

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
				if (Input.GetKeyDown (KeyCode.C)) {
                    ActiveBulletTime(!bulletTimeActive, BulletTimePriority.BulletTimePriority_High);
				}
				break;

			case inputMode.HoldToIn:
                if (Input.GetKey(KeyCode.C))
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
            if(m_bCustomizeTime == true)
            {
                Time.timeScale = m_fTimeScale;
                Time.fixedDeltaTime = m_fFixedDeltaTime;
                

            }
            else
            {
                Time.timeScale = 0.1f;
                Time.fixedDeltaTime = startDeltaTime * 0.1f;
            }
            targetTimeScale = 0.1f;
			targetDeltaTime = startDeltaTime * 0.1f;
			bulletTimePostEffect.enabled=true;
		} else {
			targetTimeScale = 1f;
			targetDeltaTime = startDeltaTime;
			bulletTimePostEffect.enabled=false;
            bulletTimeActive = false;
        }

        if(m_bDelayActive == true)
        {
            m_fCurDelayActiveTime += Time.deltaTime;
            if( m_fCurDelayActiveTime >= m_fDelayActiveTime)
            {
                m_bDelayActive = false;
                ActiveBulletTime(true, BulletTimePriority.BulletTimePriority_Low);
            }
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
                m_bDelayActive = false;
                m_bCustomizeTime = false;
            }
            else
            {
                m_curPriority = ePriority;
            }
        }
    }
    public void SetCustomizeTime(float fTimeScale, float fixedDeltaTime)
    {
        m_fFixedDeltaTime = fixedDeltaTime;
        m_fTimeScale = fTimeScale;
        m_bCustomizeTime = true;
    }

    public void DelayActive(float fTime)
    {
        if(m_bDelayActive == false)
        {
            m_fDelayActiveTime = fTime;
            m_bDelayActive = true;
            m_fCurDelayActiveTime = 0.0f;
        }
    }
}