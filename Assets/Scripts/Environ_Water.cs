using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environ_Water : MonoBehaviour
{
    // Start is called before the first frame update

    public float waterFlowPower;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<Thing>()!=null || other.tag == "player")
        {
            if (other.GetComponent<Rigidbody2D>()!=null)
            {
                var rb = other.GetComponent<Rigidbody2D>();
                rb.velocity+=(Vector2)this.transform.up*waterFlowPower*Time.deltaTime;
                rb.drag=0.5f;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
         if (other.GetComponent<Rigidbody2D>()!=null)
            {
                var rb = other.GetComponent<Rigidbody2D>();
                
                rb.drag=0.5f;
            }
    }
}
