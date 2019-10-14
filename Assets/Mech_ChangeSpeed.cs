using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_ChangeSpeed : MonoBehaviour
{
    [Header("Dir为零时原路反弹，否则按Dir方向弹出")]
    public Vector2 dir;
    public float speedFactor;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D col)
    {
        Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
        float speed = rb.velocity.magnitude;
        if (dir != Vector2.zero)
        {
            rb.velocity = dir * speed*speedFactor;
        }
        else if(speedFactor!=0)
        {
            rb.velocity = -rb.velocity * speedFactor;
        }
    }
}
