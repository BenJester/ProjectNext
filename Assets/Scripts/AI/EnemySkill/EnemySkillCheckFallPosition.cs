using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillCheckFallPosition : EnemySkillBase
{
    public LayerMask CheckMask;
    public float CheckDistance;
    public float CheckFallDistance;
    protected override void Start()
    {
        base.Start();
        Registe(this);
    }
    public override void CastSkill()
    {
        base.CastSkill();
    }

    private void FixedUpdate()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + transform.right * CheckDistance, transform.position + transform.right * CheckDistance - transform.up * CheckFallDistance);
    }

    public override bool IsConditionValid()
    {
        RaycastHit2D[] lstHit = Physics2D.RaycastAll(transform.position + transform.right * CheckDistance, -transform.up, CheckFallDistance, CheckMask);
        if (lstHit.Length > 0)
        {
            return false;
        }
        return true;
    }
}