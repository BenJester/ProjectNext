using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//检测是否有物体进入特定半径的圆圈
public class EnemySkillCheckObjectInCircle : EnemySkillBase
{
    [Header("检测是否有物体进入特定半径的圆圈")]
    [Tooltip("检测圆形半径")]
    public float CircleRadius;
    [Tooltip("待检测masklayer")]
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