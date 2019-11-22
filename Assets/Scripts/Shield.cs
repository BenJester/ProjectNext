using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    Thing thing;
    public string NameOfState;
    public string ParamAnimation;
    private Animator m_animator;
    private Animation m_animation;
    public bool bTest;
    void Start()
    {
        if(transform.parent != null)
        {
            thing = transform.parent.GetComponent<Thing>();
            if(thing != null)
            {
                thing.shield = gameObject;
            }
        }
        m_animator = GetComponent<Animator>();
        //m_animation = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if(thing != null)
        {
            if (!thing.hasShield)
            {
                Deactivate();
            }
            else
            {
                Activate();
            }
        }
        if(bTest == true)
        {
            Activate();
            bTest = false;
        }
    }

    public void Deactivate()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }

    public void Activate()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        //m_animation.Play();
        //m_animator.CrossFade(NameOfState,0);
        m_animator.SetInteger(ParamAnimation, 1);
    }
}
