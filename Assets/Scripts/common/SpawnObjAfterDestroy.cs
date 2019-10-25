using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjAfterDestroy : MonoBehaviour
{
    public List<GameObject> LstSpawnObj;
    // Start is called before the first frame update
    public void SpawnObject()
    {
        foreach(GameObject objIns in LstSpawnObj)
        {
            Instantiate(objIns, transform.position, Quaternion.identity);
        }
    }
}
