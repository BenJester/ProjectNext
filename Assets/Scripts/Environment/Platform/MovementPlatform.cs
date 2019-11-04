using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlatform : MonoBehaviour
{
    public Transform TransEndPos;

    public float MoveSpeed;
    private Vector3 m_vecStartPos;
    private Vector3 m_vecEndPos;

    private bool m_bStartToEnd;
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
        if(m_bStartToEnd == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_vecEndPos, MoveSpeed * Time.deltaTime);
            if(Vector3.Distance(transform.position, m_vecEndPos) <= 0.1f )
            {
                m_bStartToEnd = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, m_vecStartPos, MoveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, m_vecStartPos) <= 0.1f)
            {
                m_bStartToEnd = true;
            }
        }
    }
}
