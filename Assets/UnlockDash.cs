using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDash : MonoBehaviour
{
    private Dash dash;
    private void Start()
    {
        dash = GameObject.FindGameObjectWithTag("player").GetComponent<Dash>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "pawdlayer")
        {
            dash.maxCharge = 1;
        }
    }
}
