using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDoubleSwap : MonoBehaviour
{
    public bool DoubleSwap;
    public GameObject DoubleSwapMarker;
    public bool SwapDamage;
    public UnityEvent MarkerDestroy;
    private Thing m_thingDoubleSwap;
    private Swap m_swap;
    private bool m_bDoubleSwap;
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
            DoubleSwapMarker.gameObject.SetActive(false);
        }
    }

    public void SetDoubleSwapObject(Thing objThing)
    {
        if(DoubleSwapMarker != null)
        {
            if (DoubleSwap == true)
            {
                if (m_bDoubleSwap == false)
                {
                    m_thingDoubleSwap = objThing;
                    GameObjectUtil.SafeSetActive(true, DoubleSwapMarker);
                    objThing.RegisteDestroyNotify(_swapThingDestroy);
                    DoubleSwapMarker.transform.SetParent(null);
                    DoubleSwapMarker.transform.position = new Vector3(objThing.transform.position.x, objThing.transform.position.y, -1);
                }
                else
                {
                    m_thingDoubleSwap = null;
                    GameObjectUtil.SafeSetActive(false, DoubleSwapMarker);
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if( DoubleSwap == true )
        {
            if(m_thingDoubleSwap != null)
            {
                if(DoubleSwapMarker != null)
                {
                    DoubleSwapMarker.transform.position = new Vector3(m_thingDoubleSwap.transform.position.x, m_thingDoubleSwap.transform.position.y, -1);
                }
            }
        }
    }
    private void _swapThingDestroy()
    {
        m_thingDoubleSwap = null;
        if(DoubleSwapMarker != null)
        {
            GameObjectUtil.SafeSetActive(false, DoubleSwapMarker);
        }
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
                m_bDoubleSwap = true;
                m_swap.swapDamageOn = SwapDamage;
                m_swap.Do();
                m_swap.ResetSwapDamageOn();
                m_bDoubleSwap = false;
            }
        }
    }

    public void MarkerDestroyFunction()
    {
        DoubleSwapMarker = null;
    }
}
