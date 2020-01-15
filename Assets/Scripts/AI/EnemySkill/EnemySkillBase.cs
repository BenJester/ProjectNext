﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySkillBase : MonoBehaviour
{
    [Tooltip("技能名，相同类型可以为不同技能名，但是技能名必须唯一。")]
    public string NameOfSkill;

    [Tooltip("如果是持续技能的话，只有调用SetSkillCasting(false)才会中断技能返回true。" +
        "非持续性技能，释放完后立即返回结果。")]
    public bool ContinuousSkill;

    private EnemySkillManager m_skillMgr;
    private bool m_bSkillCasting;
    protected Transform m_transPlayer;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
        m_transPlayer = GlobalVariable.GetPlayer().transform;
        if (m_transPlayer == null)
        {
            Debug.Assert(false);
        }
    }

    public virtual void CastSkill()
    {
        if(ContinuousSkill == true)
        {
            m_bSkillCasting = true;
        }
        else
        {
            m_bSkillCasting = false;
        }
    }
    public virtual bool IsConditionValid()
    {
        return true;
    }

    public virtual void EndSkill()
    {
        SetSkillCasting(false);
    }

    public void Registe(EnemySkillBase _childSkill)
    {
        m_skillMgr = GetComponent<EnemySkillManager>();
        if (m_skillMgr != null)
        {
            m_skillMgr.RegisteSkill(NameOfSkill, this);
        }
    }
    public bool IsSkillCasting()
    {
        return m_bSkillCasting;
    }
    public void SetSkillCasting(bool bCasting)
    {
        m_bSkillCasting = bCasting;
    }
}
