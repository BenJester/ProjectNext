using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ti_Missel_TriggerPoint : MonoBehaviour
{
    public Ti_Missel missel;
    bool triggered=false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (triggered) return;
        if (missel.isTrigger && (collision.transform.CompareTag("floor") || collision.transform.CompareTag("thing")||collision.transform.CompareTag("player")))
        {
            triggered = true;
            StartCoroutine(missel.Explode());
        }
    }
}
