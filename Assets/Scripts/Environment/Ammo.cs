using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public int ammo;
    BoxCollider2D box;
    SpriteRenderer sr;
    private void Start()
    {
        box = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        Hostage hostage = col.GetComponent<Hostage>();
        if (hostage != null)
        {
            hostage.ammo += ammo;
            box.enabled = false;
            sr.enabled = false;
        }
    }
}
