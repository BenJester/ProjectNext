using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlatform : MonoBehaviour
{
    public Transform TransEndPos;
    public float accelerateSpeed;
    public float MoveSpeed;

    public bool IsSyncing;

    public PlayerControl1 playerCtrl;
    private Vector3 m_vecStartPos;
    private Vector3 m_vecEndPos;

    private bool m_bStartToEnd;
    private float m_curAccelerate;
    // Start is called before the first frame update
    void Start()
    {
        m_vecStartPos = transform.position;
        m_vecEndPos = TransEndPos.position;
        m_bStartToEnd = true;
    }

    // Update is called once per frame
    void Update()
    {
        float fAccelerateValue = m_curAccelerate + accelerateSpeed * Time.deltaTime;
        float fMoveSpeedTime = MoveSpeed * Time.deltaTime;

        Vector2 vecDir;
        if (m_bStartToEnd == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_vecEndPos, fMoveSpeedTime + fAccelerateValue);
            m_curAccelerate += fAccelerateValue;
            if (Vector3.Distance(transform.position, m_vecEndPos) <= 0.1f )
            {
                m_bStartToEnd = false;
                m_curAccelerate = 0;
            }
            vecDir = m_vecEndPos - transform.position;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, m_vecStartPos, fMoveSpeedTime + fAccelerateValue);
            m_curAccelerate += fAccelerateValue;
            if (Vector3.Distance(transform.position, m_vecStartPos) <= 0.1f)
            {
                m_bStartToEnd = true;
            }
            vecDir = m_vecStartPos - transform.position;
        }
        if(IsSyncing == true)
        {
            vecDir = vecDir.normalized;
            Vector2 vecDiff = vecDir.normalized * fMoveSpeedTime;
            playerCtrl.SetMoveVelocity(vecDiff );
        }
    }
}
