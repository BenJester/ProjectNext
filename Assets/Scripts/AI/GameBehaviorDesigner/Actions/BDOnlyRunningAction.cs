using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
[TaskCategory("Game/Tasks/Actions")]
public class BDOnlyRunningAction : Action
{
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Running;
    }
    public override void OnStart()
    {
    } 
}