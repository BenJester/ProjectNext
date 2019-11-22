using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillAnimation : EnemySkillBase
{
    public AnimationPlayManager AniManager;
    private bool m_bHasEventRegiste;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Registe(this);
    }
    public override void CastSkill()
    {
        base.CastSkill();
        AniManager.PlayAnimation(NameOfSkill);
        if(m_bHasEventRegiste == false)
        {
            AniManager.RegisteAnimationOverEvent(NameOfSkill, _animationFinished);
            m_bHasEventRegiste = true;
        }
    }
    private void _animationFinished()
    {
        SetSkillCasting(false);
    }
}