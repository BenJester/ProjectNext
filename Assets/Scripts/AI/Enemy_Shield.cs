using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shield : MonoBehaviour
{
    Enemy enemy;
    float offsetX;
    SpriteRenderer sr;
    Thing thing;
    BoxCollider2D box;

    private void Start()
    {
        enemy = transform.parent.GetComponent<Enemy>();
        offsetX = transform.localPosition.x;
        sr = GetComponent<SpriteRenderer>();
        thing = transform.parent.GetComponent<Thing>();
        box = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (enemy != null)
        {
            if (thing.dead)
            {
                box.enabled = false;
                return;
            }
                
            transform.localPosition = new Vector3(enemy.faceRight ? offsetX : -offsetX, transform.localPosition.y, transform.localPosition.z);
            sr.flipX = enemy.faceRight;
            if (thing.swapping)
                box.enabled = false;
            else
                box.enabled = true;
        }
    }
}
