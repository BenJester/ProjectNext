using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollision : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("floor"))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public void Update()
    {
        //transform.position = Vector3.zero;

    }
}

