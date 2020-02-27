using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPhysics : MonoBehaviour
{
    Rigidbody2D rb;
    Vector3 prevPos;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        //if (Vector3.Distance(prevPos, transform.position) > 100f)
        //{
        //    transform.position = Vector3.zero;
        //    rb.velocity = Vector2.zero;
        //}
        //prevPos = transform.position;
    }
    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("thing") && !col.collider.GetComponent<Thing>().hasShield && !col.collider.CompareTag("player"))
        {
            rb.velocity = Vector2.zero;
        }
        else if (col.collider.CompareTag("floor") || (col.collider.CompareTag("thing") && col.collider.GetComponent<Thing>().hasShield))
            rb.velocity = Vector2.zero;
    }
}
