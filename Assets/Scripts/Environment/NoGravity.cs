using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGravity : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            PlayerControl1.Instance.rb.gravityScale = 0f;
            PlayerControl1.Instance.GetComponent<Thing>().isStandardGravity = false;
            PlayerControl1.Instance.speed = 0f;
            Energy.Instance.swapCost = 0f;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            PlayerControl1.Instance.rb.gravityScale = 165f;
            PlayerControl1.Instance.GetComponent<Thing>().isStandardGravity = true;
            PlayerControl1.Instance.speed = 440f;
            Energy.Instance.swapCost = 21f;
        }

    }
}
