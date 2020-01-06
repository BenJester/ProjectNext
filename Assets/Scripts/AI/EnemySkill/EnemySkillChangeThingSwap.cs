using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillChangeThingSwap : EnemySkillBase
{
    public bool SwapStatus;
    private Thing m_thing;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Registe(this);
        m_thing = GetComponent<Thing>();
        if (m_thing == null)
        {
            Debug.Assert(false);
        }
    }
    public override void CastSkill()
    {
        base.CastSkill();
        m_thing.SetThingSwap(SwapStatus);
    } 
}