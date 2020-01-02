using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillMove : EnemySkillBase
{
    public float MoveForwardForce;
    public float MoveTime;
    private bool m_bMoving;
    private float m_fMovingTime;
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
        m_bMoving = true;
        m_fMovingTime = 0.0f;
    }
    private void FixedUpdate()
    {
        if (m_bMoving == true)
        {
            m_fMovingTime += Time.fixedDeltaTime;
            m_rigid.AddForce(transform.right * MoveForwardForce);
            if (m_fMovingTime >= MoveTime)
            {
                m_bMoving = false;
            }
        }
    }
}