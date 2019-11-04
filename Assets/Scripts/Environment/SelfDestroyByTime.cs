using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyByTime : MonoBehaviour
{
    public float DestroyTime;
    private float m_fDestroyTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        m_fDestroyTime += Time.fixedDeltaTime;
        if( DestroyTime <= m_fDestroyTime)
        {
            Destroy(gameObject);
        }
    }
    
}
