using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ben;

public class SpikeDynamic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("thing") || collision.gameObject.CompareTag("player"))
        {
            Thing colThing = collision.gameObject.GetComponent<Thing>();

            if (colThing.GetComponent<Enemy>() != null)
            {
                colThing.GetComponent<Enemy>().TakeDamage(1);
            }
            if (colThing.type == Type.player)
            {
                colThing.GetComponent<PlayerControl1>().Die();
                //StartCoroutine(colThing.GetComponent<PlayerControl1>().DelayRestart());
            }
            if (colThing.type == Type.box)
            {
                collision.GetComponent<Box>().GetSpike();
                

            }
        }
    }
}
