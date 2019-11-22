using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayComponent : MonoBehaviour
{
    public AnimationData AnimationData;
    public string NameOfAnimation;

    private AnimationPlayManager m_aniManager;
    private Animator m_animator;
    // Start is called before the first frame update
    void Start()
    {
        AnimationData.Generate();
        m_animator = GetComponent<Animator>();
        m_aniManager = GetComponent<AnimationPlayManager>();
        m_aniManager.RegisteSkill(NameOfAnimation, this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAnimation()
    {
        AnimationData.PlayAnimation(m_animator);
    }
}
