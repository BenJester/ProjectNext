using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//自翻转节点。如果是左就到右，如果是右就到左。
public class EnemySkillFlipSelf : EnemySkillBase
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
        if( transform.eulerAngles.y == 0)
        {
            transform.localRotation = Quaternion.AngleAxis(180, Vector2.up);
        }
        else if (transform.eulerAngles.y == 180)
        {
            transform.localRotation = Quaternion.AngleAxis(0, Vector2.up);
        }
    }
}