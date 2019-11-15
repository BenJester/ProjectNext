using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("Game/Tasks/Conditionals")]
public class BDCheckEnemyDead : Conditional
{
    public bool AliveCheck;
    private Thing m_thing;
    public override TaskStatus OnUpdate()
    {
        if(AliveCheck == true)
        {
            if (m_thing.dead == true)
            {
                return TaskStatus.Failure;
            }
            return TaskStatus.Success;
        }
        else
        {
            if (m_thing.dead == true)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
    public override void OnStart()
    {
        m_thing = GetComponent<Thing>();
        if(m_thing == null)
        {
            Debug.Assert(false);
        }
    }
}
