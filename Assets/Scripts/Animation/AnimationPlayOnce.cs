using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayOnce : MonoBehaviour
{
    public string NameOfAnimation;
    private Animator m_Animator;
    private float m_fCurrentLength;
    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_fCurrentLength = m_Animator.GetCurrentAnimatorStateInfo(0).length;
        m_Animator.Play(NameOfAnimation);
    }

    // Update is called once per frame
    void Update()
    {
        if( m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 )
        {
            Destroy(gameObject);
        }
    }
}
