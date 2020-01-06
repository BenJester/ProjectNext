using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillCheckThingSwap : EnemySkillBase
{
    private Thing m_thing;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Registe(this);
        m_thing = GetComponent<Thing>();
        if(m_thing == null)
        {
            Debug.Assert(false);
        }
    }
    public override void CastSkill()
    {
        base.CastSkill();
    }

    public override bool IsConditionValid()
    {
        return m_thing.IsThingSwap();
    }
}