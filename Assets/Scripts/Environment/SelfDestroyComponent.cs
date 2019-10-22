using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelfDestroyComponent : MonoBehaviour
{
    private UnityAction m_act;
    private float m_fSelfTime;
    private float m_fCurrentTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetSelfDestroyTime(float fTime, UnityAction acDestroy)
    {
        m_fSelfTime = fTime;
        m_act = acDestroy;
    }

    // Update is called once per frame
    void Update()
    {
        m_fCurrentTime += Time.deltaTime;
        if(m_fCurrentTime >= m_fSelfTime)
        {
            m_act.Invoke();
            Destroy(gameObject);
        }
    }
}
