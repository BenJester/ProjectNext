using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoubleSwap : MonoBehaviour
{
    public bool DoubleSwap;
    public GameObject DoubleSwapMarker;
    private Thing m_thingDoubleSwap;
    private Swap m_swap;
    //private Thing m_objDoubleSwap;
    // Start is called before the first frame update
    void Start()
    {
        if (DoubleSwap == false)
        {
            DoubleSwapMarker.gameObject.SetActive(false);
        }
        else
        {
            m_swap = GetComponent<Swap>();
        }
    }

    public void SetDoubleSwapObject(Thing objThing)
    {
        if( DoubleSwap == true )
        {
            m_thingDoubleSwap = objThing;
            GameObjectUtil.SafeSetActive(true, DoubleSwapMarker);
            objThing.RegisteDestroyNotify(_swapThingDestroy);
            DoubleSwapMarker.transform.SetParent(null);
            DoubleSwapMarker.transform.position = objThing.transform.position;
        }
    }
    private void FixedUpdate()
    {
        if( DoubleSwap == true )
        {
            if(m_thingDoubleSwap != null)
            {
                DoubleSwapMarker.transform.position = m_thingDoubleSwap.transform.position;
            }
        }
    }
    private void _swapThingDestroy()
    {
        m_thingDoubleSwap = null;
        GameObjectUtil.SafeSetActive(false, DoubleSwapMarker);
    }

    public bool CanDoubleSwap()
    {
        return m_thingDoubleSwap != null;
    }

    public void DoDoubleSwap()
    {
        if(DoubleSwap == true)
        {
            if (m_thingDoubleSwap != null)
            {
                m_swap.col = m_thingDoubleSwap.GetComponent<Collider2D>();
                m_swap.Do();
            }
        }
    }
}
