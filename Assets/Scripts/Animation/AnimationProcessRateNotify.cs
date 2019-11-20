using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AnimationProcessRateData
{
    public string NameOfAnimation;
    [Range(0,1)]
    public float RateOfProcess;
    public UnityEvent AnimationOverEvent;

    private bool m_bExecute;
    private int m_hashAnimation;
    public AnimationProcessRateData()
    {
    }
    public void SetExecute(bool bRes)
    {
        m_bExecute = bRes;
        m_hashAnimation = Animator.StringToHash(NameOfAnimation);
    }
    public void JudgeProcess(float fRate, int nHash)
    {
        if ( nHash == m_hashAnimation )
        {
            float _valAfterDecimal = fRate - Mathf.Floor(fRate);
            //Debug.Log(string.Format("_valAfterDecimal {0}", _valAfterDecimal));
            if (m_bExecute == true)
            {
                if(_valAfterDecimal >= 0.0f && _valAfterDecimal <= 0.02f)
                {
                    m_bExecute = false;
                }
            }
            if (m_bExecute == false)
            {
                if (_valAfterDecimal >= RateOfProcess)
                {
                    m_bExecute = true;
                    if (AnimationOverEvent != null)
                    {
                        AnimationOverEvent.Invoke();
                    }
                }
            }
        }
    }
}
public class AnimationProcessRateNotify : MonoBehaviour
{
    public List<AnimationProcessRateData> LstRateData;
    private bool m_bWatch;
    private Animator m_animator;
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        _resetData();
    }
    private void _resetData()
    {
        foreach (AnimationProcessRateData _data in LstRateData)
        {
            _data.SetExecute(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bWatch == true)
        {
            int _hashName = m_animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
            float fProcess = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            //Debug.Log(string.Format("_valAfterDecimal {0}", m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime));
            foreach (AnimationProcessRateData _data in LstRateData)
            {
                _data.JudgeProcess(fProcess, _hashName);
            }
        }
    }
    public void StartWatch(bool bWatch)
    {
        if (m_bWatch == false && bWatch == true)
        {
            _resetData();
        }
        m_bWatch = bWatch;
    }
}
