using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFix : MonoBehaviour
{
    // Start is called before the first frame update

public float speedCheck=100;
    bool isStop;
    Rigidbody2D  myrigidbody;
    void Start()
    {
        myrigidbody=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // if(checkSpeed() && other.GetComponent<Thing>()!=null){
        //     other.transform.SetParent(transform);
        // }
        if(other.tag=="floor"){
            myrigidbody.velocity=Vector2.zero;
        }

    }





   
}
