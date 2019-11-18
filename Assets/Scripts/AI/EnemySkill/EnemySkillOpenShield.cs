using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillOpenShield : EnemySkillBase
{
    private Thing m_thing;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Registe(this);
        m_thing = GetComponent<Thing>();
    }
    public override void CastSkill()
    {
        base.CastSkill();
        m_thing.hasShield = true;
    } 
}