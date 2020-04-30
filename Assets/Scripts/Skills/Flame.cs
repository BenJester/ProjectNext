using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Flame : MonoBehaviour
{
    public bool active;
    public AudioClip clip;
    AudioSource source;
    SpriteRenderer sr;
    public GameObject particle;
    public GameObject light;
    

    void Start()
    {
        source = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Activate()
    {
        source.PlayOneShot(clip);
        active = true;
        sr.color = Color.white;
        GameObject part1 = Instantiate(particle, transform.position, Quaternion.identity);
        light.SetActive(true);
        Destroy(part1, 1.5f);
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
