using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePillarKill : MonoBehaviour
{
    public bool active;
    private void OnTriggerStay2D(Collider2D col)
    {
        if (!active) return;
        if (col.CompareTag("player"))
        {
            col.GetComponent<PlayerControl1>().Die();
        }
        if (col.CompareTag("thing") && col.GetComponent<Enemy>())
        {
            col.GetComponent<Enemy>().TakeDamage(999);
        }
    }
}
