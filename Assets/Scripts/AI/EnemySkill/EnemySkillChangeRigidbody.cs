using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//变更rigidbody的属性。
//此技能可以进行扩展，调用技能后，通过填写的属性，更改rigidbody的属性。
public class EnemySkillChangeRigidbody : EnemySkillBase
{
    [Header("变更rigidbody的属性。")]
    [Tooltip("类型")]
    public RigidbodyType2D bodyTyp;
    [Tooltip("质量")]
    public float DynamicMass;
    [Tooltip("重力")]
    public float DynamicGravity;

    private Rigidbody2D m_rigidbody;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Registe(this);
        m_rigidbody = GetComponent<Rigidbody2D>();
        if(m_rigidbody == null)
        {
            Debug.Assert(false, "EnemySkillChangeRigidbody rigidbody is null");
        }
    }
    public override void CastSkill()
    {
        base.CastSkill();
        m_rigidbody.bodyType = bodyTyp;
        if (bodyTyp == RigidbodyType2D.Dynamic)
        {
            m_rigidbody.mass = DynamicMass;
            m_rigidbody.gravityScale = DynamicGravity;
        }
    } 
}