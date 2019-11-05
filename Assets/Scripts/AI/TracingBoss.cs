using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracingBoss : MonoBehaviour
{
    public float DistanceDashPlayer;
    public float DistanceDelta;
    private MovingObjectByWayPoint m_movingObj;
    private bool m_bDash;
    // Start is called before the first frame update
    void Start()
    {
        m_movingObj = GetComponent<MovingObjectByWayPoint>();
    }

    private void FixedUpdate()
    {
        if(m_bDash == false)
        {
            float fDistancePlayer = Vector2.Distance(transform.position, GlobalVariable.GetPlayer().transform.position);
            if (fDistancePlayer <= DistanceDashPlayer)
            {
                m_bDash = true;
                m_movingObj.enabled = false;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, GlobalVariable.GetPlayer().transform.position, DistanceDelta * Time.fixedDeltaTime);
        }
    }
}
