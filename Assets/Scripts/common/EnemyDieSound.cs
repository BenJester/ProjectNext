using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieSound : MonoBehaviour
{
    public int CountsOfLimit = 4;
    public float TimeLimit = 1;
    public bool Original;
    private List<float> m_lstTime;
    private int m_nCurCounts;
    // Start is called before the first frame update
    void Start()
    {
        m_lstTime = new List<float>();
    }
    private void FixedUpdate()
    {
        int nRemoveIdx = -1;
        foreach(float fTime in m_lstTime)
        {
            if(fTime + TimeLimit < Time.time)
            {
                nRemoveIdx++;
                break;
            }
        }
        if(nRemoveIdx >= 0)
        {
            m_lstTime.RemoveAt(nRemoveIdx);
        }
    }
    public bool CanPlaySound()
    {
        if(Original == true)
        {
            return true;
        }
        bool bRes = false;
        if (m_lstTime.Count < CountsOfLimit)
        {
            bRes = true;
            m_lstTime.Add(Time.time);
        }
        return bRes;
    }
    //public bool CanPlaySound()
    //{
    //    bool bRes = false;
    //    if( m_lstTime.Find(x => x == Time.time) == 0)
    //    {
    //        bRes = true;
    //    }
    //    return bRes;
    //}
}
