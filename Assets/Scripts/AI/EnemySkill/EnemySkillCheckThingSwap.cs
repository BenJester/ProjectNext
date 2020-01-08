using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//检测物体是否被交换过
public class EnemySkillCheckThingSwap : EnemySkillBase
{
    [Header("检测物体是否被交换过")]
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