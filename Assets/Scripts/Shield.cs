using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    Thing thing;
    public string NameOfState;
    public string ParamAnimation;
    public bool ActiveSelf;
    private Animator m_animator;
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
        if(ActiveSelf == true)
        {
            Activate();
        }
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
        m_animator.SetInteger(ParamAnimation, 1);
    }
}
