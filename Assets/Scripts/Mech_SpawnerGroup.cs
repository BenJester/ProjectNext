using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_SpawnerGroup : MonoBehaviour
{
    public List<Mech_EnemySpawner> spawnerList;

    void Start()
    {
        
    }

    public bool CheckAllDead()
    {
        foreach (var spawner in spawnerList)
        {
            if (!spawner.spawning) return false;
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
