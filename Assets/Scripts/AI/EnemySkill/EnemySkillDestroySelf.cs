using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillDestroySelf : EnemySkillBase
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Registe(this);
    }
    public override void CastSkill()
    {
        base.CastSkill();
        Destroy(gameObject);
    } 
}