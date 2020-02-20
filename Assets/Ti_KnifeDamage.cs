using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ti_KnifeDamage : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("player"))
        {
            //other.transform.GetComponent<PlayerControl1>().Die();
        }
        if (other.transform.CompareTag("thing") && other.transform.GetComponent<Enemy>())
        {
            other.transform.GetComponent<Enemy>().TakeDamage(1);
        }
    }
}
