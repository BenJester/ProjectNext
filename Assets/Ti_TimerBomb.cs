﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ti_TimerBomb : TriggerItem_Base
{
    [Header("炸弹被点燃")]
    public bool isTrigger = false;

    [Header("炸弹被点燃后爆炸的时间")]

    public float triggerTime = 3;
    
    public float explosionRadius;
    public int damage = 1;
    private float tempTimer;

    public GameObject areaIndicator;
    Thing thing;
    bool triggered;
    void Start()
    {
        thing = GetComponent<Thing>();
        thing.OnDie += Explode;
    }

    void Update()
    {
         if(isTrigger && tempTimer <= Time.time){
            Explode();
            isTrigger = false;
            Destroy(gameObject);
        }
    }

    public override void HandleKickTrigger(){
        if(!isTrigger) TriggerBomb();
        
    } 
    public override void HandleSwapTrigger(){
        if(!isTrigger) TriggerBomb();
        
    } 

    void TriggerBomb(){
        isTrigger=true;
        GetComponent<SpriteRenderer>().color = Color.red;
        tempTimer = Time.time + triggerTime;
    }
    
    public void Explode()
    {
        if (triggered) return;
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        GameObject area = Instantiate(areaIndicator, transform.position, Quaternion.identity);
        area.transform.parent = null;
        area.GetComponent<SpriteRenderer>().size = new Vector2(explosionRadius * 2,explosionRadius * 2);
        Destroy(area, 0.1f);
        foreach (var col in cols)
        {
            if (col.CompareTag("player"))
            {
                col.GetComponent<PlayerControl1>().Die();
            }
            if (col.CompareTag("thing") && col.GetComponent<Enemy>())
            {
                col.GetComponent<Enemy>().TakeDamage(damage);              
            }
        }
        triggered = true;
        if (!thing.dead)
            thing.Die(); 
        
    }
}
