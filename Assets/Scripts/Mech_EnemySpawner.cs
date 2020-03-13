using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_EnemySpawner : MonoBehaviour
{
    public Thing thing;
    public float cooldown;

    public Thing currThing;
    bool spawning;

    private void Update()
    {
        if (!spawning && (currThing == null || currThing.dead))
        {
            spawning = true;
            StartCoroutine(DoSpawn());
        }
    }
    
    IEnumerator DoSpawn()
    {
        yield return new WaitForSeconds(cooldown);
        Spawn();
        spawning = false;
    }

    void Spawn()
    {
        currThing = Instantiate(thing.gameObject, transform.position, Quaternion.identity).GetComponent<Thing>();

    }
}
