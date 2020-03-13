using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_EnemySpawner : MonoBehaviour
{
    public List<Thing> thingList;
    public List<Transform> posList;
    public int count;
    public int maxNum;
    public float cooldown;

    public Thing currThing;
    bool spawning;
    bool finished;
    private void Update()
    {
        if (!spawning && !finished && (currThing == null || currThing.dead))
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
        currThing = Instantiate(thingList[Random.Range(0, thingList.Count)].gameObject, posList[Random.Range(0, posList.Count)].position, Quaternion.identity).GetComponent<Thing>();
        count += 1;
        if (count >= maxNum)
            finished = true;
    }
}
