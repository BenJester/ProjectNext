using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectComponent : MonoBehaviour
{
    public GameObject TargetPrefab;
    public float TimeToSpawn;
    public int CountsOfSpawn;
    public float SelfDestroyTime;

    private float m_fCurrentTime;
    private int m_nCounts;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_nCounts < CountsOfSpawn)
        {
            m_fCurrentTime += Time.deltaTime;
            if (m_fCurrentTime >= TimeToSpawn)
            {
                m_fCurrentTime -= TimeToSpawn;
                _spawn();
            }
        }
        else if( CountsOfSpawn < 0)
        {
            m_fCurrentTime += Time.deltaTime;
            if (m_fCurrentTime >= TimeToSpawn)
            {
                m_fCurrentTime -= TimeToSpawn;
                _spawn();
            }
        }
    }
    private void _ObjDestroy()
    {
        m_nCounts--;
        m_fCurrentTime = 0.0f;
    }

    private void _spawn()
    {
        GameObject objIns = Instantiate(TargetPrefab, Vector3.zero, Quaternion.identity);
        objIns.transform.parent = transform;
        m_nCounts++;
        if (SelfDestroyTime > 0.0f)
        {
            SelfDestroyComponent objSelfDestroy = objIns.AddComponent<SelfDestroyComponent>() as SelfDestroyComponent;
            objSelfDestroy.SetSelfDestroyTime(SelfDestroyTime, _ObjDestroy);
        }
    }
}
