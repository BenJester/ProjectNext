using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnTrigger : MonoBehaviour
{


    public List<Mech_EnemySpawner> spawnerList;
    public UnityEvent triggerEvent;
    public bool isTrigger = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (checkSpawner()) {
            triggerEvent.Invoke();
        }
    }

    public bool checkSpawner()
    {
        foreach (var spawner in spawnerList)
        {
            if (!spawner.spawningNotFirstTime) return false;
        }
        foreach (var spawner in spawnerList)
        {
            spawner.end = true;
        }
        return true;
    }
}
