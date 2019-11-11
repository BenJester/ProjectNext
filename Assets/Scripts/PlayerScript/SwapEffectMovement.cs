using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapEffectMovement : MonoBehaviour
{
    public float MovingSpeed;
    public float DistanceOffset;

    public Transform PlayerTrans;

    private Vector3 m_vecOriginal;
    private bool m_bMoving;
    private Vector2 m_vecDst;
    private Vector2 m_vecSrc;
    // Start is called before the first frame update
    void Start()
    {
        m_vecOriginal = transform.localPosition;
        transform.SetParent(null);
    }

    private void FixedUpdate()
    {
        if( m_bMoving == true )
        {
            Vector2 vecCurPos = Vector2.MoveTowards(transform.position, m_vecDst, MovingSpeed * Time.fixedDeltaTime);
            float fDistance = Vector2.Distance(vecCurPos, transform.position);
            if( fDistance <= DistanceOffset)
            {
                //transform.position = m_vecDst;
                transform.localPosition = m_vecOriginal;
                m_bMoving = false;
                transform.SetParent(null);
            }
            else
            {
                transform.position = vecCurPos;
            }
        }
        else
        {
            if(PlayerTrans != null)
            {
                transform.position = PlayerTrans.position;
            }
        }
    }
    public void StartMoving(Vector2 vecDstPos,Vector2 vecSrcPos)
    {
        transform.SetParent(PlayerTrans);
        m_bMoving = true;
        m_vecDst = vecDstPos;
        m_vecSrc = vecSrcPos;
        transform.localPosition = new Vector3(0,0,0);
    }
}
