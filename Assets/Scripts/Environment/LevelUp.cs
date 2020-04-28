using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    public float bulletSpeed;
    public float shootInterval;
    public GameObject bullet;
    public int ammo;
    BoxCollider2D box;
    SpriteRenderer sr;
    Thing thing;
    AudioSource source;
    public AudioClip sound;
    public GameObject particle;
    public AudioClip spawnSound;
    private void Start()
    {
        box = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        thing = GetComponent<Thing>();
        source = GetComponent<AudioSource>();
        source.PlayOneShot(spawnSound);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        Hostage hostage = col.collider.GetComponent<Hostage>();
        if (hostage != null)
        {
            hostage.bulletSpeed = bulletSpeed;
            hostage.shootInterval = shootInterval;
            hostage.bullet = bullet;
            hostage.ammo += ammo;
            hostage.transform.localScale *= 1.5f;
            thing.Die();
            source.PlayOneShot(sound);
            source.PlayOneShot(spawnSound);
            GameObject part1 = Instantiate(particle, transform.position, Quaternion.identity);
            Destroy(part1, 1.5f);
        }
    }
}
