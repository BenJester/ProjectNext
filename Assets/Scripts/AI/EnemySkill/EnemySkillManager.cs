using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillManager : MonoBehaviour
{

    private Dictionary<string, EnemySkillBase> m_dic;
    // Start is called before the first frame update
    void Start()
    {
    }
    public void RegisteSkill(string strNameOfSkill, EnemySkillBase _skillBase)
    {
        if( m_dic == null )
        {
            m_dic = new Dictionary<string, EnemySkillBase>();
        }
        if(m_dic.ContainsKey(strNameOfSkill) == false)
        {
            m_dic.Add(strNameOfSkill, _skillBase);
        }
        else
        {
            Debug.Assert(false, string.Format("技能已经被注册过了"));
        }
    }
    public void CastSkill(string strNameOfSkill)
    {
        EnemySkillBase _skill;
        if (m_dic.TryGetValue(strNameOfSkill, out _skill))
        {
            _skill.CastSkill();
        }
    }
    public bool IsSkillCasting(string strNameOfSkill)
    {
        bool bCasting = false;
        EnemySkillBase _skill;
        if (m_dic.TryGetValue(strNameOfSkill, out _skill))
        {
            bCasting = _skill.IsSkillCasting();
        }
        else
        {
            Debug.Assert(false);
        }
        return bCasting;
    }
}
