using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillMove : EnemySkillBase
{
    public float MoveForwardForce;
    public float MoveTime;
    public bool RightDir;
    public bool AlwaysRight;
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
        m_fMovingTime = 0.0f;
    }
    public override void EndSkill()
    {
        base.EndSkill();
        SetSkillCasting(false);
    }
    private void FixedUpdate()
    {
        if (IsSkillCasting() == true)
        {
            m_fMovingTime += Time.fixedDeltaTime;
            if(AlwaysRight == false)
            {
                if (RightDir == true)
                {
                    m_rigid.MovePosition(transform.position + Vector3.right * MoveForwardForce * Time.fixedDeltaTime);
                }
                else
                {
                    m_rigid.MovePosition(transform.position + -Vector3.right * MoveForwardForce * Time.fixedDeltaTime);
                }
            }
            else
            {
                m_rigid.MovePosition(transform.position + transform.right * MoveForwardForce * Time.fixedDeltaTime);
            }
            if (m_fMovingTime >= MoveTime)
            {
                SetSkillCasting(false);
            }
        }
    }
}