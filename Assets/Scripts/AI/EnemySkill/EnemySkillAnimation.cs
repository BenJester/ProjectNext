using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//技能播放动画，此技能名，就是动画名。
public class EnemySkillAnimation : EnemySkillBase
{
    [Header("技能播放动画，此技能名，就是动画名。")]
    [Tooltip("通过依赖注入的方式解决有些动画可能不是在自己对象上的问题")]
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