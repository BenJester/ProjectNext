using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
[TaskCategory("Game/Tasks/Conditionals")]
public class BDCheckEnemyHealth : Conditional
{
    public float HealthCheck;
    private Enemy m_enemy;
    public override TaskStatus OnUpdate()
    {
        if(m_enemy.health >= HealthCheck )
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
    public override void OnStart()
    {
        m_enemy = GetComponent<Enemy>();
    } 
}