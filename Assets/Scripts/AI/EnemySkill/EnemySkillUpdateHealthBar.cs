using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillUpdateHealthBar : EnemySkillBase
{
    private Enemy m_enemy;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Registe(this);
        m_enemy = GetComponent<Enemy>();
        if(m_enemy == null)
        {
            Debug.Assert(false);
        }
    }
    public override void CastSkill()
    {
        base.CastSkill();
        UIBossHealth _health = FindObjectOfType<UIBossHealth>();
        if (_health != null)
        {
            _health.UpdateHealth(m_enemy.health / m_enemy.maxHealth);
        }
        else
        {
            Debug.Assert(false);
        }
    } 
}