using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayComponent : MonoBehaviour
{
    public AnimationData AnimationData;
    private Animator m_animator;
    public string NameOfAnimation;
    // Start is called before the first frame update
    void Start()
    {
        AnimationData.Generate();
        m_animator = GetComponent<Animator>();
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
