using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillCheckObjectInCircle : EnemySkillBase
{
    public float CircleRadius;
    public LayerMask CheckMask;
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, CircleRadius);
    }
    public override bool IsConditionValid()
    {
        Collider2D[] lstHit = Physics2D.OverlapCircleAll(transform.position, CircleRadius, CheckMask);
        if (lstHit.Length > 0)
        {
            return true;
        }
        return false;
    }
}