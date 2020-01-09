using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillOpenBossHealthBar : EnemySkillBase
{
    public bool OpenBar;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Registe(this);
    }
    public override void CastSkill()
    {
        base.CastSkill();
        UIBossHealth _health = FindObjectOfType<UIBossHealth>();
        if(_health != null)
        {
            _health.OpenBossHealthBar(OpenBar);
        }
        else
        {
            Debug.Assert(false);
        }
    } 
}