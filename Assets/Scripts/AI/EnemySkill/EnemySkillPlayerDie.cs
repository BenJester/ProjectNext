using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//执行玩家立即死亡节点
public class EnemySkillPlayerDie : EnemySkillBase
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

        if( PlayerControl1.Instance != null )
        {
            PlayerControl1.Instance.PlayerDieImmediately();
        }
        else
        {
            Debug.Assert(false, "Player dose not find.");
        }
    } 
}