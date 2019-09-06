using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : Skill
{

    public float range;
    public float throwSpeed;
    Collider2D col;


    public override bool Check()
    {
        if (playerControl.closestObjectToPlayer && playerControl.closestPlayerDistance <= range)
        {
            col = playerControl.closestObjectToPlayer.GetComponent<Collider2D>();
            return true;
        }
        else
            return false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            Do();
    }

    public override void Do()
    {
        if (!active || !Check())
            return;
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mouseWorldPos - (Vector2)player.transform.position).normalized;
        col.gameObject.GetComponent<Rigidbody2D>().velocity = dir * throwSpeed;
    }
}
