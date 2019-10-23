using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBossJump : MonoBehaviour
{
    public float JumpUPForce;
    public float JumpForwardForce;
    public float JumpTime;

    public bool DebugTest;

    private float m_fJumpingTime;
    private bool m_bJumping;

    private Rigidbody2D m_rigid;
    // Start is called before the first frame update
    void Start()
    {
        m_rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (DebugTest == true)
        {
            JumpBoss();
        }
        if (m_bJumping == true)
        {
            m_fJumpingTime += Time.fixedDeltaTime;
            m_rigid.AddForce(Vector2.up * JumpUPForce);
            m_rigid.AddForce(transform.right * JumpForwardForce);
            if (m_fJumpingTime >= JumpTime)
            {
                m_bJumping = false;
            }
        }
    }
    public void JumpBoss()
    {
        DebugTest = false;
        m_bJumping = true;
        m_fJumpingTime = 0.0f;
    }
    public void StopJump()
    {
        m_bJumping = false;
    }
}
