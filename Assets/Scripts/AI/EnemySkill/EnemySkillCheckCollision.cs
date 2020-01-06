using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillCheckCollision : EnemySkillBase
{
    public LayerMask CheckMask;
    public float CheckDistance;
    public float AngleToWatch;
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