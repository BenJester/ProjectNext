using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwapEffectMovement : MonoBehaviour
{
    public float MovingSpeed;
    public float DistanceOffset;

    public Transform PlayerTrans;

    public UnityEvent DestroyInvoke;
    private bool m_bMoving;
    private Vector2 m_vecDst;
    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(null);
    }

    private void FixedUpdate()
    {
        if( m_bMoving == true )
        {
            Vector2 vecCurPos = Vector2.MoveTowards(transform.position, m_vecDst, MovingSpeed * Time.fixedDeltaTime);
            float fDistance = Vector2.Distance(m_vecDst, transform.position);
            if( fDistance <= DistanceOffset)
            {
                m_bMoving = false;
                if(DestroyInvoke != null)
                {
                    DestroyInvoke.Invoke();
                }
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
        transform.SetParent(null);
        transform.position = vecSrcPos;
        m_bMoving = true;
        m_vecDst = vecDstPos;
    }
}
