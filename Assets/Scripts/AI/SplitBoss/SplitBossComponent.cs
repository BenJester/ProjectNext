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

    public List<SplitBossPart> LstProcessPart;

    public List<Transform> LstBossPart;

    public bool SplitStart;
    public bool SplitBack;

    private bool m_bRight;
    void Start()
    {
        LstProcessPart.Add(TransHead);
        LstProcessPart.Add(TransBody);
        LstProcessPart.Add(TransLeftHand);
        LstProcessPart.Add(TransRightHand);
        LstProcessPart.Add(TransLeftFeet);
        LstProcessPart.Add(TransRightFeet);
        FlipBoss(false);
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
        foreach (SplitBossPart _part in LstProcessPart)
        {
            _part.BackToOriginalPos();
        }
    }

    public void SplitReady()
    {
        foreach(SplitBossPart _part in LstProcessPart)
        {
            _part.SplitReady();
        }
    }

    public void FlipBoss(bool bRight)
    {
        m_bRight = bRight;
        if (bRight == true)
        {
            transform.localRotation = Quaternion.AngleAxis(0, Vector2.up);
        }
        else
        {
            transform.localRotation = Quaternion.AngleAxis(180, Vector2.up);
        }
    }
    public bool BossRight()
    {
        return m_bRight;
    }
}
