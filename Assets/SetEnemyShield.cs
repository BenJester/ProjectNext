using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEnemyShield : MonoBehaviour
{

    public AudioSource asr;
    // Start is called before the first frame update
    void Start()
    {
        asr = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Thing col = collision.GetComponent<Thing>();
        if (col != null) {
            if (col.type == Ben.Type.enemy && col.hasShield)
            {
                col.SetShield(false);
                asr.Play();
            }

        }
        
    }
}
