using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//扣玩家血节点
public class EnemySkillPlayerReduceHP : EnemySkillBase
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

        if (PlayerControl1.Instance != null)
        {
            PlayerControl1.Instance.Die();
        }
        else
        {
            Debug.Assert(false, "Player dose not find.");
        }
    } 
}