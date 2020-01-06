using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillJump : EnemySkillBase
{
    public float JumpUPForce;
    public float JumpForwardForce;
    public float JumpTime;
    private float m_fJumpingTime;
    private bool m_bJumping;

    private Rigidbody2D m_rigid;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Registe(this);
        m_rigid = GetComponent<Rigidbody2D>();
    }
    public override void CastSkill()
    {
        base.CastSkill();
        m_bJumping = true;
        m_fJumpingTime = 0.0f;
    }
    private void FixedUpdate()
    {
        if (m_bJumping == true)
        {
            m_fJumpingTime += Time.fixedDeltaTime;
            m_rigid.AddForce(Vector2.up * JumpUPForce);
            m_rigid.AddForce(transform.right * JumpForwardForce);
            if (m_fJumpingTime >= JumpTime)
            {
                m_bJumping = false;
                SetSkillCasting(false);
            }
        }
    }
}