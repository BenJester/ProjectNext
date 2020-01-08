using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//检测是否有类型为Thing物体碰撞此物体
public class EnemySkillCollisionThing : EnemySkillBase
{
    [Header("检测是否有类型为Thing物体碰撞此物体")]
    private bool m_bResult;
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Thing>() != null)
        {
            m_bResult = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Thing>() != null)
        {
            m_bResult = true;
        }
    }

    public override bool IsConditionValid()
    {
        return m_bResult;
    }
}