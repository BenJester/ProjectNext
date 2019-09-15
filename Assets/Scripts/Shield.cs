using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    Thing thing;

    void Start()
    {
        thing = transform.parent.GetComponent<Thing>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!thing.hasShield)
        {
            Deactivate();
        }
        else
        {
            Activate();
        }
    }

    public void Deactivate()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }

    public void Activate()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }
}
