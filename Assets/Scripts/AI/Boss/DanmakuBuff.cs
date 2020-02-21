using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanmakuBuff : MonoBehaviour
{
    public bool isHeart;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("player"))
        {
            PlayerControl1 player = other.GetComponent<PlayerControl1>();
            if (player.GetComponent<Rigidbody2D>().velocity.magnitude < 2500f)
                return;
            if (isHeart)
            {
                
                player.hp += 1;
                player.maxhp += 1;
            }
            else
            {
                other.GetComponent<ThrowKunai>().kunai1.swapDamage += 1;
                other.GetComponent<ThrowKunai>().kunai2.swapDamage += 1;
            }
            Destroy(gameObject);
        }
        if (other.transform.CompareTag("floor"))
        {
            Destroy(gameObject);
        }
    }
}
