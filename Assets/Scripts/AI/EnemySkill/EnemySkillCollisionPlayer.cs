using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//检测判断玩家是否碰撞此物体
public class EnemySkillCollisionPlayer : EnemySkillBase
{
    [Header("检测判断玩家是否碰撞此物体")]
    private bool m_bResult;
    // Start is called before the first frame update
    private bool m_bChecking;
    protected override void Start()
    {
        base.Start();
        Registe(this);
    }
    public override void CastSkill()
    {
        base.CastSkill();
        m_bResult = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(GlobalTagDefine.TagName_player) == true )
        {
            m_bResult = true;
        }
    }

    public override bool IsConditionValid()
    {
        return m_bResult;
    }
}