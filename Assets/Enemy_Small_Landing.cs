using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Small_Landing : Enemy
{
    // Start is called before the first frame update

    GameObject player;
    public float speed;

    void Start()
    {
        player = PlayerControl1.Instance.gameObject;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x > transform.position.x)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
    }
}
