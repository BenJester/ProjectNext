using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_EnemySpawner : MonoBehaviour
{
    public Thing thing;
    public float cooldown;

    public Thing currThing;

    void Spawn()
    {
        currThing = Instantiate(thing.gameObject, transform.position, Quaternion.identity).GetComponent<Thing>();

    }
}
