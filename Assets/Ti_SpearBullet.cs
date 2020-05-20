﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ti_SpearBullet : MonoBehaviour
{
    // Start is called before the first frame update

    Transform player;
    Rigidbody2D rb;
    Thing thing;
    public float speed;
    public PlayerShoot ps;
    public Vector2 spriteOffset;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        thing = GetComponent<Thing>();
        player = PlayerControl1.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot() {


        Vector2 dir = (Vector2)player.position+ spriteOffset - (Vector2)transform.position;
        dir = -dir.normalized;
        
        
       transform.localRotation = Quaternion.Euler(0, 0, -Dash.AngleBetween(Vector2.up, dir));
        ps.Shoot(dir);



    }
}
