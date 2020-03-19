using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_Revelocity : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            Rigidbody2D rb= collision.GetComponent<Rigidbody2D>();
            
            float originalVelocity = rb.velocity.y;
            if(originalVelocity<0) rb.velocity = transform.right * originalVelocity * -1.5f;
            


        }
    }
}
