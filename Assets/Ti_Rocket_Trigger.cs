using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ti_Rocket_Trigger : MonoBehaviour
{
    public Ti_Rocket rocket;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (rocket.isTrigger && (collision.transform.CompareTag("floor") || collision.transform.CompareTag("thing")))
        {
            StartCoroutine(rocket.Explode());
        }
    }
}
