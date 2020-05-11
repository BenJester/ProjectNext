using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area_Acceleration : MonoBehaviour
{
    // Start is called before the first frame update

    public float velocityMultiple=1.05f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Thing>()!=null)
        {
            collision.GetComponent<Rigidbody2D>().velocity *= velocityMultiple;
        }


    }
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.tag == "player")
    //    {
    //        collision.GetComponent<Rigidbody2D>().velocity *= velocityMultiple;
    //    }
        

    //}
}
