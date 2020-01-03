using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
[TaskCategory("Game/Tasks/Actions")]
public class BDEnemyCastSkill : Action
{
    public string NameOfSkill;
    private EnemySkillManager m_skillMgr;
    public override TaskStatus OnUpdate()
    {
        if(m_skillMgr.IsSkillCasting(NameOfSkill) == true)
        {
            return TaskStatus.Running;
        }
        else
        {
            return TaskStatus.Success;
        }
    }
    public override void OnAwake()
    {
        m_skillMgr = GetComponent<EnemySkillManager>();
    }
    public override void OnStart()
    {
        m_skillMgr.CastSkill(NameOfSkill);
    }

    public override void OnEnd()
    {
        m_skillMgr.EndSkill(NameOfSkill);
    }
}