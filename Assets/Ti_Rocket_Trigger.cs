using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ti_Rocket_Trigger : MonoBehaviour
{
    public Ti_Rocket rocket;
    bool triggered;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (triggered) return;
        if (rocket.isTrigger && (collision.transform.CompareTag("floor") || collision.transform.CompareTag("thing")))
        {
            triggered = true;
            StartCoroutine(rocket.Explode());
        }
    }
}
