using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBossBehavior : MonoBehaviour
{
    public int JumpingCounts;
    public float JumpingTime;
    public float TimeToShootAfterJump;

    private bool m_bJumppingRight;
    private int m_nCurJumpingCounts;
    private float m_fCurJumpingTime;
    private SplitBossJump m_jump;
    private SplitBossComponent m_bossCom;
    private SplitBossShoot m_shootBoss;
    private bool m_bJumping;
    private float m_fCurrentReadyShoot;
    private bool m_bShoot;
    // Start is called before the first frame update
    void Start()
    {
        m_jump = GetComponent<SplitBossJump>();
        m_bossCom = GetComponent<SplitBossComponent>();
        m_shootBoss = GetComponent<SplitBossShoot>();
        m_jump.JumpBoss();
        m_bJumping = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bJumppingRight == true)
        {

        }
    }

    private void FixedUpdate()
    {
        if(m_bJumping == true)
        {
            if (m_nCurJumpingCounts >= JumpingCounts)
            {
                //m_nCurJumpingCounts = 0;
                m_bossCom.FlipBoss(!m_bossCom.BossRight());
                m_jump.StopJump();
                m_bJumping = false;
                m_bShoot = true;
            }
            else
            {
                m_fCurJumpingTime += Time.fixedDeltaTime;
                if (m_fCurJumpingTime >= JumpingTime)
                {
                    m_fCurJumpingTime -= JumpingTime;
                    m_nCurJumpingCounts++;
                    m_jump.JumpBoss();
                }
            }
        }
        if(m_bShoot == true)
        {
            m_fCurrentReadyShoot += Time.fixedDeltaTime;
            if( m_fCurrentReadyShoot >= TimeToShootAfterJump )
            {
                m_shootBoss.BossShoot();
                m_bShoot = false;
            }
        }
    }
}
