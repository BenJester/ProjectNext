﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_UnstableWindHole : MonoBehaviour
{

    public float windHolePower;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<Thing>()!=null)
        {
            if (other.GetComponent<Rigidbody2D>()!=null)
            {
                if (other.tag == "player") {
                    PlayerControl1.Instance.disableAirControl=true;
                }
                other.GetComponent<Rigidbody2D>().velocity+=(Vector2)this.transform.up*-windHolePower*Time.deltaTime;
            }
        }
    }
}
