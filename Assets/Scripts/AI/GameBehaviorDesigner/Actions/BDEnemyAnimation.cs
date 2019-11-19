using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
[TaskCategory("Game/Tasks/Actions")]
public class BDEnemyAnimation : Action
{
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
    public override void OnStart()
    {
    } 
}