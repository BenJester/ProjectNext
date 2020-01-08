using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//检测掉落判定
public class EnemySkillCheckFallPosition : EnemySkillBase
{
    [Header("检测掉落判定")]
    [Tooltip("待检测masklayer")]
    public LayerMask CheckMask;
    [Tooltip("检测距离")]
    public float CheckDistance;
    [Tooltip("检测高度")]
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