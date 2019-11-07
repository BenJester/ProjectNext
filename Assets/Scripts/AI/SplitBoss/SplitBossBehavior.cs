using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBossBehavior : MonoBehaviour
{
    public int JumpingCounts;
    public float JumpingTime;
    public float TimeToShootAfterJump;
    public List<SplitBossPart> LstParts;

    private bool m_bJumppingRight;
    private int m_nCurJumpingCounts;
    private float m_fCurJumpingTime;
    private SplitBossJump m_jump;
    private SplitBossComponent m_bossCom;
    private SplitBossShoot m_shootBoss;
    private EnemySkillShoot m_shootSkill;
    private bool m_bJumping;
    private float m_fCurrentReadyShoot;
    public bool m_bShoot;

    public bool testCallBack;
    // Start is called before the first frame update
    void Start()
    {
        m_jump = GetComponent<SplitBossJump>();
        m_bossCom = GetComponent<SplitBossComponent>();
        m_shootBoss = GetComponent<SplitBossShoot>();
        m_shootSkill = GetComponent<EnemySkillShoot>();
        //m_jump.JumpBoss();
        //m_bJumping = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bShoot == true)
        {
            m_bShoot = false;
            m_shootSkill.CastSkill();
            //Debug.Assert(false);
        }
    }

    private void FixedUpdate()
    {
        if(m_bShoot == true)
        {

        }
        //if(m_bJumping == true)
        //{
        //    if (m_nCurJumpingCounts >= JumpingCounts)
        //    {
        //        //m_nCurJumpingCounts = 0;
        //        m_bossCom.FlipBoss(!m_bossCom.BossRight());
        //        m_jump.StopJump();
        //        m_bJumping = false;
        //        m_bShoot = true;
        //    }
        //    else
        //    {
        //        m_fCurJumpingTime += Time.fixedDeltaTime;
        //        if (m_fCurJumpingTime >= JumpingTime)
        //        {
        //            m_fCurJumpingTime -= JumpingTime;
        //            m_nCurJumpingCounts++;
        //            m_jump.JumpBoss();
        //        }
        //    }
        //}
        //if(m_bShoot == true)
        //{
        //    m_fCurrentReadyShoot += Time.fixedDeltaTime;
        //    if( m_fCurrentReadyShoot >= TimeToShootAfterJump )
        //    {
        //        m_shootBoss.BossShoot();
        //        m_bShoot = false;
        //    }
        //}

        //if(testCallBack == true)
        //{
        //    testCallBack = false;
        //    foreach (SplitBossPart _part in LstParts)
        //    {
        //        _part.ThingEnable(false);
        //        _part.GetComponent<RotateByTarget>().StopRotate();
        //        _part.GetComponent<SplitCallBackParts>().StartCallBack(_part.TransOriginalSplit.transform, 1);
        //    }
        //}
    }
}
