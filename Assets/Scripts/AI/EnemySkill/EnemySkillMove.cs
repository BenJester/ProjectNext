using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillMove : EnemySkillBase
{
    [Header("移动")]
    [Tooltip("移动超前力")]
    public float MoveForwardForce;
    [Tooltip("移动力释放时间")]
    public float MoveTime;
    [Tooltip("左右方向")]
    public bool RightDir;
    [Tooltip("一直朝前标记")]
    public bool AlwaysRight;
    [Tooltip("是否适用动画曲线")]
    public bool UsingAnimationCurve;
    [Tooltip("使用animatiCurve的曲线")]
    public AnimationCurve AniCurve;
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
            if(UsingAnimationCurve == true)
            {
                float fRate = m_fMovingTime / MoveTime;
                float fCurveValue = AniCurve.Evaluate(fRate);
                float fForce = fCurveValue * MoveForwardForce;
                m_rigid.MovePosition(transform.position + Vector3.right * fForce * Time.fixedDeltaTime);
            }
            else
            {
                if (AlwaysRight == false)
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
            }
            if (m_fMovingTime >= MoveTime)
            {
                SetSkillCasting(false);
            }
        }
    }
}