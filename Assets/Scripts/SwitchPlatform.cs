using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SwitchPlatform : MonoBehaviour
{
    public bool worldOne;
    TilemapRenderer tr;
    TilemapCollider2D box;
    public int swapNum;

    void Start()
    {
        tr = GetComponent<TilemapRenderer>();
        box = GetComponent<TilemapCollider2D>();
        PlayerControl1.Instance.swap.OnSwap += HandleSwap;
        if (!worldOne)
            Hide();
    }

    void HandleSwap()
    {
        swapNum += 1;
        if ((worldOne && swapNum % 2 == 0) || (!worldOne && swapNum % 2 == 1))
        {
            Display();
        }
        else
        {
            Hide();
        }
    }

    void Hide()
    {
        tr.enabled = false;
        //box.enabled = false;
        gameObject.layer = 18;
    }

    void Display()
    {
        tr.enabled = true;
        //box.enabled = true;
        gameObject.layer = 8;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
