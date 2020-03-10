using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHeart : MonoBehaviour
{
    AudioClip hitClip;
    AudioSource source;
    private void Start()
    {
        hitClip = Resources.Load<AudioClip>("Sounds/Toy_PopGun_Shot");
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("player"))
        {
            PlayerControl1 player = other.GetComponent<PlayerControl1>();

            if (player.hp < player.maxhp)
                player.hp += 1;
                

            PlayerControl1.Instance.GetComponent<AudioSource>().PlayOneShot(hitClip);
            Destroy(gameObject);
        }
        if (other.transform.CompareTag("thing"))
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (enemy.health < enemy.maxHealth)
                    enemy.health += 1;
            }
            PlayerControl1.Instance.GetComponent<AudioSource>().PlayOneShot(hitClip);
            Destroy(gameObject);
        }
    }
}
