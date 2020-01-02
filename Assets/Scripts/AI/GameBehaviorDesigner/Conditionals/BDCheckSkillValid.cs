using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
[TaskCategory("Game/Tasks/Conditionals")]
public class BDCheckSkillValid : Conditional
{
    public string NameOfSkill;
    private EnemySkillManager m_skillMgr;
    public override TaskStatus OnUpdate()
    {
        if (m_skillMgr.IsSkillConditionValid(NameOfSkill) == true)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }

    public override void OnAwake()
    {
        m_skillMgr = GetComponent<EnemySkillManager>();
    }
    public override void OnStart()
    {
    }
}