﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TriggerArea : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isTrigger = false;
    public UnityEvent trigger;
    SpriteRenderer spr;
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTrigger && collision.tag == "player")
        {
            spr.color = Color.yellow;
            isTrigger = true;
            trigger.Invoke();
        }
    }
}
