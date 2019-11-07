using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandDisappearPlatform : MonoBehaviour
{
    public string AnimationNameOfShaking;
    public string AnimationNameOfDestroy;
    public string AnimationNameOfShow;
    public bool DisableForever;

    private Animator m_animator;
    private string m_strShaking = "Shaking";
    private string m_strDestroy = "Destroy";
    private string m_strShow    = "Show";
    private string m_strDisappear    = "Disappear";
    private int m_hashNameShaking;
    private int m_hashNameDestroy;
    private int m_hashNameShow;
    private BoxCollider2D m_collider;

    private int m_lastHash;

    private bool m_bPlayAnimation;
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_collider = GetComponent<BoxCollider2D>();

        m_hashNameShaking   = Animator.StringToHash(AnimationNameOfShaking);
        m_hashNameDestroy   = Animator.StringToHash(AnimationNameOfDestroy);
        m_hashNameShow      = Animator.StringToHash(AnimationNameOfShow);
    }

    // Update is called once per frame
    void Update()
    {
        {
            if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !m_animator.IsInTransition(0))
            {
                if (m_animator.GetCurrentAnimatorStateInfo(0).shortNameHash == m_hashNameShaking && m_lastHash != m_hashNameShaking)
                {
                    m_animator.SetInteger(m_strShaking, 0);
                    m_animator.SetInteger(m_strDestroy, 1);
                    m_animator.SetInteger(m_strShow, 0);
                    m_collider.enabled = false;
                    m_lastHash = m_hashNameShaking;
                }
                else if (m_animator.GetCurrentAnimatorStateInfo(0).shortNameHash == m_hashNameDestroy && m_lastHash != m_hashNameDestroy)
                {
                    if( DisableForever == true )
                    {
                        m_animator.SetInteger(m_strShaking, 0);
                        m_animator.SetInteger(m_strDestroy, 0);
                        m_animator.SetInteger(m_strShow, 0);
                        m_animator.SetInteger(m_strDisappear, 1);
                    }
                    else
                    {
                        m_animator.SetInteger(m_strShaking, 0);
                        m_animator.SetInteger(m_strDestroy, 0);
                        m_animator.SetInteger(m_strShow, 1);
                    }
                    m_lastHash = m_hashNameDestroy;
                }
                else if (m_animator.GetCurrentAnimatorStateInfo(0).shortNameHash == m_hashNameShow && m_lastHash != m_hashNameShow)
                {
                    if (DisableForever == false)
                    {

                    }
                    m_animator.SetInteger(m_strShaking, 0);
                    m_animator.SetInteger(m_strDestroy, 0);
                    m_animator.SetInteger(m_strShow, 0);
                    m_collider.enabled = true;
                    m_lastHash = m_hashNameShow;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if( collision.gameObject.CompareTag(GlobalTagDefine.TagName_player) == true )
        {
            Collider2D _collider = collision.gameObject.GetComponent<Collider2D>();
            float fPos = _collider.gameObject.transform.position.y - _collider.bounds.size.y / 2;
            if(fPos > transform.position.y )
            {
                m_animator.SetInteger(m_strShaking, 1);
                m_animator.SetInteger(m_strDestroy, 0);
                m_bPlayAnimation = true;
            }
        }
    }


}
