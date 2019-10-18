using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBossComponent : MonoBehaviour
{
    // Start is called before the first frame update
    public SplitBossPart TransHead;
    public SplitBossPart TransBody;
    public SplitBossPart TransLeftHand;
    public SplitBossPart TransRightHand;
    public SplitBossPart TransLeftFeet;
    public SplitBossPart TransRightFeet;

    public List<SplitBossPart> LstSplitBossPart;

    public bool SplitStart;
    public bool SplitBack;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(SplitStart == true)
        {
            SplitStart = false;
            SplitReady();
        }
        if(SplitBack == true)
        {
            SplitBack = false;
            SplitBackToPos();
        }
    }

    public void SplitBackToPos()
    {
        foreach (SplitBossPart _part in LstSplitBossPart)
        {
            _part.BackToOriginalPos();
        }
    }

    public void SplitReady()
    {
        foreach(SplitBossPart _part in LstSplitBossPart)
        {
            _part.SplitReady();
        }
    }
}
