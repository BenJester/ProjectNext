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
    public bool spawning;
    public bool spawningNotFirstTime;
    bool firstTime;
    bool finished;
    public bool end;
    public GameObject SpawnHint;
    Vector3 nextPos;

    private void Update()
    {
        if (!spawning && !finished && (currThing == null || currThing.dead))
        {
            if (firstTime)
                spawningNotFirstTime = true;
            if (!firstTime)
            {
                firstTime = true;
            }
            spawning = true;
            StartCoroutine(DoSpawn());
        }
    }
    
    IEnumerator DoSpawn()
    {
        SpawnHint.SetActive(true);
        
        nextPos = posList[Random.Range(0, posList.Count)].position;
        SpawnHint.transform.position = nextPos;
        yield return new WaitForSeconds(cooldown);
        Spawn();
        SpawnHint.SetActive(false);
        spawning = false;
        spawningNotFirstTime = false;
    }

    void Spawn()
    {
        if (end)
        {
            finished = true;
            return;
        }
        currThing = Instantiate(thingList[Random.Range(0, thingList.Count)].gameObject, nextPos, Quaternion.identity).GetComponent<Thing>();
        count += 1;
        if (count >= maxNum)
            finished = true;
    }
}
