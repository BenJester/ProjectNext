using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGravity : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            foreach (var thing in PlayerControl1.Instance.thingList)
            {
                thing.body.gravityScale = thing.gravity;
            }
            PlayerControl1.Instance.rb.gravityScale = PlayerControl1.Instance.GetComponent<Thing>().gravity;
        }
        PlayerControl1.Instance.GetComponent<InvertGravity>().even = false;
    }
}
