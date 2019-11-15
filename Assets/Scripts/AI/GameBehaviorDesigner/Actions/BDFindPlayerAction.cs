using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("Game/Tasks/Actions")]
public class BDFindPlayerAction : Action
{
    public SharedTransform PlayerTransform;
    public override TaskStatus OnUpdate()
    {
        if (PlayerTransform.Value == null)
        {
            PlayerTransform.Value = GlobalVariable.GetPlayer().transform;
            if(PlayerTransform.Value != null)
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
            return TaskStatus.Success;
        }
    }
}
