using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
[TaskCategory("Game/Tasks/Conditionals")]
public class BDIsEnemyHealthLowerThan : Conditional
{
    public float JudgeHealthValue;
    private Enemy m_enemy;
    public override TaskStatus OnUpdate()
    {
        if (m_enemy.health < JudgeHealthValue)
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