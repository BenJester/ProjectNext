using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillFlip : EnemySkillBase
{
    public bool RightDir;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Registe(this);
    }
    public override void CastSkill()
    {
        base.CastSkill();
        if(RightDir == true)
        {
            transform.localRotation = Quaternion.AngleAxis(0, Vector2.up);
        }
        else
        {
            transform.localRotation = Quaternion.AngleAxis(180, Vector2.up);
        }
    } 
}