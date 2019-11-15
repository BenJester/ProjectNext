using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillBase : MonoBehaviour
{
    public string NameOfSkill;

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
        m_bSkillCasting = true;
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
    public void SetSkillCastring(bool bCasting)
    {
        m_bSkillCasting = bCasting;
    }
}
