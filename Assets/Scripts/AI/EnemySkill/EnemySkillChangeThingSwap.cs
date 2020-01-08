using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//设置交换物的状态为false。true的时候会在物体交换后设置。
public class EnemySkillChangeThingSwap : EnemySkillBase
{
    [Header("设置交换物的状态为false。true的时候会在物体交换后设置。")]
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
        m_thing.SetThingSwap(false);
    } 
}