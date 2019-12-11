using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoubleSwap : MonoBehaviour
{
    public bool DoubleSwap;
    public GameObject DoubleSwapMarker;
    // Start is called before the first frame update
    void Start()
    {
        if (DoubleSwap == false)
        {
            DoubleSwapMarker.gameObject.SetActive(false);
        }
    }
    public void ProcessDoubleSwap(Swap _swap)
    {
        if (DoubleSwap == true)
        {
            if (_swap.CanDoubleSwap())
            {
                if (_swap.col != null && !_swap.col.GetComponent<Thing>().dead)
                {
                    GameObjectUtil.SafeSetActive(true, DoubleSwapMarker);
                    DoubleSwapMarker.transform.position = new Vector3(_swap.col.transform.position.x, _swap.col.gameObject.transform.position.y, -1f);
                }
                else
                {
                    GameObjectUtil.SafeSetActive(false, DoubleSwapMarker);
                }
            }
            else
            {
                GameObjectUtil.SafeSetActive(false, DoubleSwapMarker);
            }
        }
    }
}
