using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillAnimation : EnemySkillBase
{
    public AnimationPlayComponent AniPlayCom;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Registe(this);
    }
    public override void CastSkill()
    {
        base.CastSkill();
    } 
}