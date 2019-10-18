using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBossPart : MonoBehaviour
{
    public Transform TransReadySplit;
    private Vector3 m_vecOriginalPos;

    private Thing m_thing;
    private Box m_box;
    private BoxCollider2D m_collider;
    private Rigidbody2D m_rigid;
    private PlatformEffector2D m_effector2d;
    // Start is called before the first frame update
    void Start()
    {
        m_vecOriginalPos = transform.localPosition;
        m_thing         = GetComponent<Thing>();
        m_box           = GetComponent<Box>();
        m_collider      = GetComponent<BoxCollider2D>();
        m_rigid         = GetComponent<Rigidbody2D>();
        m_effector2d    = GetComponent<PlatformEffector2D>();
        _ThingEnable(false);
    }

    public void SplitReady()
    {
        if (TransReadySplit == true)
        {
            transform.localPosition = TransReadySplit.localPosition;
            //_ThingEnable(true);
        }
    }

    public void BackToOriginalPos()
    {
        if (TransReadySplit == true)
        {
            transform.localPosition = m_vecOriginalPos;
            _ThingEnable(false);
        }
    }

    private void _ThingEnable(bool bEnable)
    {
        if (TransReadySplit != null)
        {
            if (m_rigid != null)
            {
                m_rigid.isKinematic = !bEnable;
            }
            if (m_collider != null)
            {
                m_collider.enabled = bEnable;
            }
            if (m_thing != null)
            {
                m_thing.enabled = bEnable;
            }
            if (m_box != null)
            {
                m_box.enabled = bEnable;
            }
            if (m_effector2d != null)
            {
                m_effector2d.enabled = bEnable;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
