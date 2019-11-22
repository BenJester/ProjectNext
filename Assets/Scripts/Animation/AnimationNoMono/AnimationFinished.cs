using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationFinished 
{
    private string m_strNameCache;
    private int m_hashAnimation;
    private int m_lastHash;
    private Animator m_animator;
    private UnityEvent m_aniOverAction;
    public AnimationFinished(Animator _animator, string strAniName, UnityEvent _overEvt)
    {
        m_animator = _animator;
        if(_animator == null)
        {
            Debug.Assert(false);
        }
        m_strNameCache = strAniName;
        m_hashAnimation = Animator.StringToHash(strAniName);
        m_aniOverAction = _overEvt;
    }
    public void AniUpdate()
    {
        if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !m_animator.IsInTransition(0))
        {
            if (m_animator.GetCurrentAnimatorStateInfo(0).shortNameHash == m_hashAnimation && m_lastHash != m_hashAnimation)
            {
                m_lastHash = m_hashAnimation;
                if (m_aniOverAction != null)
                {
                    m_aniOverAction.Invoke();
                }
            }
            else if (m_animator.GetCurrentAnimatorStateInfo(0).shortNameHash != m_lastHash)
            {
                m_lastHash = m_animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
            }
        }
    }
}
