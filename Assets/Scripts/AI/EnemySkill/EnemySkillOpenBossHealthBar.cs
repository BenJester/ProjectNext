using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillOpenBossHealthBar : EnemySkillBase
{
    public bool OpenBar;
    private UIBossHealth m_health;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Registe(this);
    }
    public override void CastSkill()
    {
        base.CastSkill();
        m_health = FindObjectOfType<UIBossHealth>();
        if(m_health != null)
        {
            m_health.OpenBossHealthBar(OpenBar);
        }
        else
        {
            Debug.Assert(false);
        }
    }

    private void OnDestroy()
    {
        if(m_health != null)
        {
            m_health.OpenBossHealthBar(false);
        }
    }
}