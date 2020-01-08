using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//执行毁掉自己的节点
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