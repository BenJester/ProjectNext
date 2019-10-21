using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBossPart : MonoBehaviour
{
    public Transform TransReadySplit;
    public Transform TransOriginalSplit;

    private Thing m_thing;
    private Box m_box;
    private BoxCollider2D m_collider;
    private Rigidbody2D m_rigid;
    private PlatformEffector2D m_effector2d;

    private Transform m_child;
    private RotateByTarget m_rotateTarget;
    private float m_fOriginalGravity;
    private SplitBossPartCollision m_collision;

    // Start is called before the first frame update
    void Start()
    {
        m_thing         = GetComponent<Thing>();
        m_box           = GetComponent<Box>();
        m_collider      = GetComponent<BoxCollider2D>();
        m_rigid         = GetComponent<Rigidbody2D>();
        m_effector2d    = GetComponent<PlatformEffector2D>();
        m_rotateTarget  = GetComponent<RotateByTarget>();
        m_collision     = GetComponent<SplitBossPartCollision>();
        if (m_rigid != null)
        {
            m_fOriginalGravity = m_rigid.gravityScale;
        }
        _ThingEnable(false);
    }

    public void SplitReady()
    {
        if (TransReadySplit == true)
        {
            transform.position = TransReadySplit.position;
            //_ThingEnable(true);
        }
    }

    public void BackToOriginalPos()
    {
        if (TransReadySplit == true)
        {
            transform.localPosition = TransOriginalSplit.localPosition;
            _ThingEnable(false);
        }
    }

    public void ThingEnable(bool bEnable)
    {
        _ThingEnable(bEnable);
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

    public void ShootReady()
    {
        //m_rigid.gravityScale = 0.0f;
        //m_rigid.bodyType = RigidbodyType2D.Dynamic;
        //m_collider.enabled = true;

        //m_child.GetComponent<RotateByTarget>().StartRotate(transform);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localRotation = Quaternion.AngleAxis(TestAngle, transform.up);
    }

    public void RotateTarget(Transform _target)
    {
        m_rotateTarget.StartRotate(_target);

        if(m_collision.IsCollision())
        {

        }
        else
        {
            if (m_rigid != null)
            {
                m_rigid.isKinematic = false;
                m_rigid.gravityScale = 0.0f;
            }
            if (m_collider != null)
            {
                m_collider.enabled = true;
            }
            if (m_thing != null)
            {
                m_thing.enabled = false;
            }
            if (m_box != null)
            {
                m_box.enabled = false;
            }
            if (m_effector2d != null)
            {
                m_effector2d.enabled = false;
            }
        }
    }

    public void SetGravity()
    {
        m_rotateTarget.StartRotate(null);
        if (m_rigid != null)
        {
            m_rigid.isKinematic = false;
            m_rigid.gravityScale = m_fOriginalGravity;
        }
    }

    public void FlipPart(bool bRight)
    {
        if (bRight == true)
        {
            transform.localRotation = Quaternion.AngleAxis(0, Vector2.up);
        }
        else
        {
            transform.localRotation = Quaternion.AngleAxis(180, Vector2.up);
        }
    }
}
