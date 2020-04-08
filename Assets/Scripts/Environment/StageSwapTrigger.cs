using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSwapTrigger : MonoBehaviour
{
    public StageSwap stageSwap;
    public bool triggered;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered && collision.CompareTag("player"))
        {
            stageSwap.Next();
            triggered = true;
        }
    }
}
