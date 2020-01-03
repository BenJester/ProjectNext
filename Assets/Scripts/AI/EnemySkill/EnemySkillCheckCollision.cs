using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillCheckCollision : EnemySkillBase
{
    public LayerMask CheckMask;
    public float CheckDistance;
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
        m_bChecking = true;
    }

    private void FixedUpdate()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * CheckDistance);
    }

    public override bool IsConditionValid()
    {
        RaycastHit2D[] lstHit = Physics2D.RaycastAll(transform.position, transform.right, CheckDistance, CheckMask);
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