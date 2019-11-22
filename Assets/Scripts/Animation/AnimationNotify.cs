using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class AnimationNotify : MonoBehaviour
{
    public string NameOfOverAnimation;
    public UnityEvent AnimationOverEvent;
    public bool CheckChild;

    private int m_hashAnimation;
    private int m_lastHash;
    private Animator m_animator;

    private AnimationFinished m_aniFinished;
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        if (m_animator == null && CheckChild == true)
        {
            m_animator = GetComponentInChildren<Animator>();
        }
        //m_hashAnimation = Animator.StringToHash(NameOfOverAnimation);
        m_aniFinished = new AnimationFinished(m_animator, NameOfOverAnimation, AnimationOverEvent);
    }

    // Update is called once per frame
    void Update()
    {
        m_aniFinished.AniUpdate();
        //if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !m_animator.IsInTransition(0))
        //{
        //    if (m_animator.GetCurrentAnimatorStateInfo(0).shortNameHash == m_hashAnimation && m_lastHash != m_hashAnimation)
        //    {
        //        m_lastHash = m_hashAnimation;
        //        if (AnimationOverEvent != null)
        //        {
        //            AnimationOverEvent.Invoke();
        //        }
        //        //Debug.Log(string.Format("anim {0}", m_lastHash));
        //    }
        //    else if (m_animator.GetCurrentAnimatorStateInfo(0).shortNameHash != m_lastHash)
        //    {
        //        m_lastHash = m_animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
        //        //Debug.Log(string.Format("anim {0}", m_lastHash));
        //    }
        //}
    }
}
