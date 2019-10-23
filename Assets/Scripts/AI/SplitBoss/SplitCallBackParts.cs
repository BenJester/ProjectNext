using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitCallBackParts : MonoBehaviour
{
    private Transform m_TransTo;
    private float m_fTimeToCallBAck;

    private float m_fCounter;
    private bool m_bStartCallBack;
    // Start is called before the first frame update
    void Start()
    {
    }
    public void StartCallBack(Transform _targetTrans,float fTimeToCallBack)
    {
        m_fCounter = 0.0f;
        m_bStartCallBack = true;
        m_TransTo = _targetTrans;
        m_fTimeToCallBAck = fTimeToCallBack;
    }
    // Update is called once per frame
    void Update()
    {
        if(m_bStartCallBack == true)
        {
            if (m_fCounter < m_fTimeToCallBAck)
            {
                m_fCounter += Time.deltaTime;
                Vector3 currentPos = transform.position;

                float speed = Vector3.Distance(currentPos, m_TransTo.position) / (m_fTimeToCallBAck - m_fCounter) * Time.deltaTime;

                transform.position = Vector3.MoveTowards(currentPos, m_TransTo.position, speed);
            }
            else
            {
                m_bStartCallBack = false;
            }
        }
    }
}
