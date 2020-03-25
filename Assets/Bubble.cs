using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    // Start is called before the first frame update
    bool isDead = false;
    Thing thisThing;
    public GameObject explodeParticle;
    void Start()
    {
        thisThing = GetComponent<Thing>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead && thisThing.dead) {

            isDead = true;
            GameObject part = explodeParticle;


        }
    }

    
}
