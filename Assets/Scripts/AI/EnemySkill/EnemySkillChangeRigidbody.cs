using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillChangeRigidbody : EnemySkillBase
{
    public RigidbodyType2D bodyTyp;

    public float DynamicMass;
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