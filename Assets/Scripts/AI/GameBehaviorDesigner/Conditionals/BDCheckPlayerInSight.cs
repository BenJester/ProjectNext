using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("Game/Tasks/Conditionals")]
public class BDCheckPlayerInSight : Conditional
{
    public SharedTransform PlayerTrans;

    private EnemyAttribute m_enemyAttribute;
    public override TaskStatus OnUpdate()
    {
        if( PlayerTrans.Value != null)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, (PlayerTrans.Value.position - transform.position).normalized, m_enemyAttribute.sightDistance, (1 << 10) | (1 << 8) | (1 << 9));
            RaycastHit2D hitNear;
            if (hits.Length >= 2)
            {
                hitNear = hits[1];
                if (hitNear.collider.CompareTag( GlobalTagDefine.TagName_player) )
                {
                    return TaskStatus.Success;
                }
                else
                {
                    return TaskStatus.Failure;
                }
            }
            else
            {
                return TaskStatus.Failure;
            }
        }
        return TaskStatus.Running;
    }

    public override void OnStart()
    {
        m_enemyAttribute = GetComponent<EnemyAttribute>();
    }
}
