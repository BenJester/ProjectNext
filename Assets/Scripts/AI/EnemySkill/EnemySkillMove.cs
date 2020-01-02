using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillMove : EnemySkillBase
{
    public float MoveForwardForce;
    public float MoveTime;
    public bool RightDir;
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
            if(RightDir == true)
            {
                transform.Translate(Vector3.right * MoveForwardForce * Time.fixedDeltaTime);
            }
            else
            {
                transform.Translate(-Vector3.right * MoveForwardForce * Time.fixedDeltaTime);
            }
            if (m_fMovingTime >= MoveTime)
            {
                m_bMoving = false;
            }
        }
    }
}