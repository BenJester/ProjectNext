using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//检测碰撞物的判断
public class EnemySkillCheckCollision : EnemySkillBase
{
    [Header("检测碰撞物的判断")]
    [Tooltip("待检测物的masklayer")]
    public LayerMask CheckMask;
    [Tooltip("检测距离")]
    public float CheckDistance;
    [Tooltip("检测角度")]
    public float AngleToWatch;
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
        Vector3 vecDir = Quaternion.AngleAxis(AngleToWatch, Vector3.forward) * transform.right;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + vecDir * CheckDistance);
    }

    public override bool IsConditionValid()
    {
        Vector3 vecDir = Quaternion.AngleAxis(AngleToWatch, Vector3.forward) * transform.right;
        RaycastHit2D[] lstHit = Physics2D.RaycastAll(transform.position, vecDir, CheckDistance, CheckMask);
        if( lstHit.Length > 0 )
        {
            foreach(RaycastHit2D _hit in lstHit)
            {
                if(_hit.collider.gameObject != transform.gameObject)
                {
                    return true;
                }
            }
            return false;
        }
        return false;
    }
}