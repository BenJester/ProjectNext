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
            DoubleSwapMarker.transform.SetParent(objThing.transform);
            DoubleSwapMarker.transform.localPosition = new Vector3(0,0,-1f);
        }
    }
    private void _swapThingDestroy()
    {
        m_thingDoubleSwap = null;
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
