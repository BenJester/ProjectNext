﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_MultiMechs_Button : MonoBehaviour
{
    public bool onlyOnce;
    public bool isPressed = false;
    public Mech_base[] mechs;
    //public LineRenderer lr;
    private Vector3 oriPos;
    private float cd;
    public bool isFinish=false;
    private bool canPressed=true;

    void Start()
    {
        //lr.SetPosition(0, transform.position);
        //lr.SetPosition(1, mech.transform.position);
        oriPos = transform.position;

    }


    void Update()
    {
        if (canPressed && isPressed)
        {
            foreach (var mech in mechs)
            {
                if (mech.type == Mech_base.MechType.Doing)
                {
                    mech.Doing();
                }
            }
           
        }

        if (cd > 0)
        {
            cd -= Time.deltaTime;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canPressed && (!isPressed && collision.tag == "thing") || (!isPressed && collision.tag == "player"))
        {
            isPressed = true;
            transform.position = oriPos - new Vector3(0, 10, 0);

            if (cd <= 0)
            {
                foreach (var mech in mechs)
                {
                    mech.DoOnce();
                    
                }
                
                if (onlyOnce)
                {
                    canPressed = false;
                }
                    
                cd = 1;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((isPressed && collision.tag == "thing") || (isPressed && collision.tag == "player") )&& !onlyOnce)
        {
            isPressed = false;
            transform.position = oriPos;
        }
    }
}
