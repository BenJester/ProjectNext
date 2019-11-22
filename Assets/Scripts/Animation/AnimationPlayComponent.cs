using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationPlayComponent : MonoBehaviour
{
    public AnimationData AnimationData;
    public string NameOfAnimation;
    public string NameInAnimationCtrl;
    private AnimationPlayManager m_aniManager;
    private Animator m_animator;
    private AnimationFinished m_aniOver;
    private UnityEvent m_AnimationFinished;
    private bool m_bPlayed;
    private bool m_bNameInCtrlValid;
    // Start is called before the first frame update
    void Start()
    {
        m_AnimationFinished = new UnityEvent();
        AnimationData.Generate();
        m_animator = GetComponent<Animator>();
        m_aniManager = GetComponent<AnimationPlayManager>();
        m_aniManager.RegisteAnimation(NameOfAnimation, this);
        m_aniOver = new AnimationFinished(m_animator, NameInAnimationCtrl, m_AnimationFinished);
        m_AnimationFinished.AddListener(_animationFinished);
        m_bNameInCtrlValid = NameInAnimationCtrl.Length > 0;
    }
    private void _animationFinished()
    {
        //m_bPlayed = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(m_bPlayed == true)
        {
            m_aniOver.AniUpdate();
        }
    }

    public void PlayAnimation()
    {
        AnimationData.PlayAnimation(m_animator);

        m_bPlayed = m_bNameInCtrlValid;
    }
    public void RegisteFinishedEvent(UnityAction acFinished)
    {
        m_AnimationFinished.AddListener(acFinished);
    }
}
