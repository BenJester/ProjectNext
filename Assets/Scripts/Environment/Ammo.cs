﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public int ammo;
    BoxCollider2D box;
    SpriteRenderer sr;
    Thing thing;
    private void Start()
    {
        box = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        thing = GetComponent<Thing>();
    }
    private void OnCollisionEnter2D (Collision2D col)
    {
        Hostage hostage = col.collider.GetComponent<Hostage>();
        if (hostage != null)
        {
            hostage.ammo += ammo;
            box.enabled = false;
            sr.enabled = false;
            thing.Die();
        }
    }
}
