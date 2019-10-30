using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_NinjaV2 : Enemy
{
    public enum State
    {
        idle = 0,
        walk = 1,
        shoot = 2,
        swap = 3,
        dash = 4,
        swapBullet = 5,
    }

    public State state;
    public bool busy;

    Transform player;
    PlayerControl1 playerControl;
    Rigidbody2D body;
    BoxCollider2D playerBox;

    public float sightDistance;

    //public GameObject bullet;
    public float bulletInstanceDistance;
    public float shootDelay;
    public int shootCount;
    public float shootInteval;
    //public float bulletSpeed;

    public float rushDelay;
    public float dashDelay;
    public int dashCount;
    public int dashRageCount;
    public float dashInteval;
    public float dashSpeed;
    public float dashDuration;
    public float hitboxWidth;

    public float throwDelay;
    public int throwCount;
    public int throwRageCount;

    public float walkSpeed;
    public float walkDuration;

    public float idleDuration;

    public float dashThreshold;
    public float shootThreshold;

    public float bombDelay;
    //public float bombSpeed;
    //public GameObject bomb;

    public float attackInteval;
    public bool justAttacked;

    public bool OnlyHorizontalDash;

    bool enraged;
    bool justEnraged;

    private EnemySkillBase m_shootSkill;
    private EnemySkillBase m_throwBomb;

    private bool m_bDashToggle;
    private bool m_bThrowBomb;
    private bool m_bShootBullet;

    private Animator m_animator;

    public string AnimatorChargingPara;
    public string AnimatorAttackPara;
    public string AnimatorThrowBombPara;
    public string AnimatorShootingPara;
    public string AnimatorThrowReadyPara;

    public string AnimationNameThrowReady;
    public string AnimationNameThrow;

    private int m_nAnimatorChargingPara;
    private int m_nAnimatorAttackPara;
    private int m_nAnimatorThrowBombPara;
    private int m_nAnimatorShootingPara;
    private int m_nAnimatorThrowReadyPara;

    private int m_nAnimationThrowReady;
    private int m_nAnimationThrow;

    private int m_lastHash;
    private bool m_bBossFlipRight;

    private int m_nBombCounts;

    public float AngleHitDiff;
    void Start()
    {
        base.Start();
        m_bDashToggle = true;
        player = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();
        playerControl = player.GetComponent<PlayerControl1>();

        body = GetComponent<Rigidbody2D>();

        playerBox = player.GetComponent<BoxCollider2D>();

        m_shootSkill = GetComponent<EnemySkillShoot>();

        m_throwBomb = GetComponent<EnemySkillThrowBomb>();

        m_animator = GetComponent<Animator>();
        m_nAnimatorChargingPara     = Animator.StringToHash(AnimatorChargingPara);
        m_nAnimatorAttackPara       = Animator.StringToHash(AnimatorAttackPara);
        m_nAnimatorThrowBombPara    = Animator.StringToHash(AnimatorThrowBombPara);
        m_nAnimatorShootingPara     = Animator.StringToHash(AnimatorShootingPara);
        m_nAnimatorThrowReadyPara = Animator.StringToHash(AnimatorThrowReadyPara);

        m_nAnimationThrowReady = Animator.StringToHash(AnimationNameThrowReady);
        m_nAnimationThrow = Animator.StringToHash(AnimationNameThrow);



    }

    private void _processAnimation()
    {
        if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !m_animator.IsInTransition(0))
        {
            if (m_animator.GetCurrentAnimatorStateInfo(0).shortNameHash == m_nAnimationThrowReady && m_lastHash != m_nAnimationThrowReady)
            {
                //丢雷
                m_throwBomb.CastSkill();
                m_lastHash = m_nAnimationThrowReady;

                m_animator.SetInteger(m_nAnimatorThrowBombPara, 1);
                m_animator.SetInteger(m_nAnimatorThrowReadyPara, 0);
            }
            else if (m_animator.GetCurrentAnimatorStateInfo(0).shortNameHash == m_nAnimationThrow && m_lastHash != m_nAnimationThrow)
            {
                m_lastHash = m_nAnimationThrow;

                m_animator.SetInteger(m_nAnimatorThrowBombPara, 0);
                m_animator.SetInteger(m_nAnimatorThrowReadyPara, 0);

                int nNeedThrowCounts = 0;
                if( enraged == true )
                {
                    nNeedThrowCounts = throwRageCount;
                }
                else
                {
                    nNeedThrowCounts = throwCount;
                }
                if( m_nBombCounts < nNeedThrowCounts )
                {
                    StartCoroutine(_bombCoroutine());
                }
                else
                {
                    //busy = false;
                    if (enraged == true)
                    {
                        m_bThrowBomb = false;
                        m_bShootBullet = true;
                    }
                    else
                    {
                        m_bDashToggle = true;
                    }
                    StartCoroutine(StartAttackTimer());
                }
            }
        }
    }
    IEnumerator _bombCoroutine()
    {
        yield return new WaitForSeconds(throwDelay);
        _throwBomb();
    }
    public bool CheckPlayerInSight()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, (player.position - transform.position).normalized, sightDistance, (1 << 10) | (1 << 8) | (1 << 9));
        RaycastHit2D hitNear;
        if (hits.Length >= 2)
        {
            hitNear = hits[1];
            if (hitNear.collider.tag == "player") return true;
            else return false;
        }
        else return false;
    }

    IEnumerator Enrage()
    {
        enraged = true;
        justEnraged = true;
        yield return new WaitForSeconds(1f);
        thing.hasShield = true;
        AnimHelper.Instance.Scale(thing.shield, 0f, 1.3f, 0.3f);
        yield return new WaitForSeconds(0.3f);
        AnimHelper.Instance.Scale(thing.shield, 1f, 0.77f, 0.1f);
        yield return new WaitForSeconds(1.8f);
        justEnraged = false;
    }
    private void _startThrowBomb()
    {
        if( m_nBombCounts == 0 )
        {
            _throwBomb();
        }
    }
    void PhaseTwoAI()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (!enraged)
        {
            justAttacked = true;
            StartCoroutine(Enrage());
        }

        if (!justAttacked && !justEnraged)
        {
            if (m_bDashToggle == true)
            {
                StartCoroutine(DashAction(dashRageCount));
            }
            else if (m_bThrowBomb == true)
            {
                _startThrowBomb();
            }
            else
            {
                justAttacked = true;
                StartCoroutine(Shoot());
            }
        }
        else
        {
            if (distance < dashThreshold)
            {
                StartCoroutine(Idle());
            }
            else
            {
                StartCoroutine(Walk());
            }
        }
    }

    void PhaseOneAI()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (!justAttacked)
        {
            if (m_bDashToggle == true)
            {
                StartCoroutine(DashAction(dashCount));
            }
            else
            {
                _startThrowBomb();
            }
        }
        else
        {
            if (distance < dashThreshold)
            {
                StartCoroutine(Idle());
            }
            else
            {
                StartCoroutine(Walk());
            }
        }
    }

    private void Update()
    {
        if (!CheckPlayerInSight() || thing.dead) return;

        if (health >= 2)
        {
            PhaseOneAI();
        }
        else
        {
            PhaseTwoAI();
        }
        _processAnimation();
    }

    IEnumerator StartAttackTimer()
    {
        justAttacked = true;
        float timer = 0f;
        while (timer < attackInteval)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        justAttacked = false;
        busy = false;
    }

    IEnumerator Idle()
    {
        if (busy) yield break;
        busy = true;

        m_animator.SetInteger(m_nAnimatorChargingPara, 0);
        m_animator.SetInteger(m_nAnimatorAttackPara, 0);
        yield return new WaitForSeconds(idleDuration);
        busy = false;
    }

    IEnumerator Walk()
    {
        if (busy) yield break;
        busy = true;

        m_animator.SetInteger(m_nAnimatorChargingPara, 0);
        m_animator.SetInteger(m_nAnimatorAttackPara, 0);

        float timer = 0f;
        body.velocity = new Vector2(player.position.x < transform.position.x ? -walkSpeed : walkSpeed, body.velocity.y);
        while (timer < walkDuration)
        {
            if (!grounded) break;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        busy = false;
    }

    IEnumerator Shoot()
    {
        if (busy) yield break;
        busy = true;
        yield return new WaitForSeconds(shootDelay);

        for (int i = 0; i < shootCount; i++)
        {
            m_shootSkill.CastSkill();
            yield return new WaitForSeconds(shootInteval);
        }
        m_bShootBullet = false;
        m_bDashToggle = true;
        busy = false;
        StartCoroutine(StartAttackTimer());
    }

    private void _throwBomb()
    {
        busy = true;
        m_animator.SetInteger(m_nAnimatorThrowReadyPara, 1);
        m_nBombCounts++;
    }


    IEnumerator DashAction(int nDashCount)
    {
        if (busy) yield break;
        busy = true;
        yield return new WaitForSeconds(dashDelay);

        //m_animator.SetInteger()

        for (int i = 0; i < nDashCount; i++)
        {

            Vector3 direction;
            if (OnlyHorizontalDash == true)
            {
                direction = (new Vector3(player.position.x, transform.position.y, player.position.z) - transform.position).normalized;
            }
            else
            {
                direction = (player.position - transform.position).normalized;
            }
            float timer = 0f;
            _processFlipBoss(true);
            m_animator.SetInteger(m_nAnimatorChargingPara, 1);
            m_animator.SetInteger(m_nAnimatorAttackPara, 0);
            while (timer < dashInteval)
            {
                body.velocity = new Vector2(0, 0);
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            m_animator.SetInteger(m_nAnimatorChargingPara, 0);
            m_animator.SetInteger(m_nAnimatorAttackPara, 1);

            yield return new WaitForSeconds(rushDelay);

            timer = 0f;
            while (timer < dashDuration)
            {
                Physics2D.IgnoreCollision(box, playerBox, true);
                body.velocity = direction * dashSpeed;
                float fAngle = Vector2.SignedAngle(transform.position, player.position);
                Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position + direction * (box.size.x + hitboxWidth),
                                 new Vector2(hitboxWidth * 2, box.size.y),
                                 fAngle + AngleHitDiff);

                foreach (Collider2D col in cols)
                {
                    if (col.CompareTag("player"))
                        playerControl.Die();
                }
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
                Physics2D.IgnoreCollision(box, playerBox, false);
            }
            timer = 0f;
            body.velocity = Vector2.zero;
        }
        m_bDashToggle = false;
        m_nBombCounts = 0;
        m_bThrowBomb = true;
        m_animator.SetInteger(m_nAnimatorChargingPara, 0);
        m_animator.SetInteger(m_nAnimatorAttackPara, 0);
        StartCoroutine(StartAttackTimer());
        busy = false;
    }

    private void _FlipBoss(bool bRight)
    {
        if (bRight == true)
        {
            transform.localRotation = Quaternion.AngleAxis(0, Vector2.up);
        }
        else
        {
            transform.localRotation = Quaternion.AngleAxis(180, Vector2.up);
        }
    }

    private void _processFlipBoss(bool bForce )
    {
        bool bRight = false;
        if(player.position.x < transform.position.x)
        {
            bRight = true;
        }
        else
        {
            bRight = false;
        }
        if(bForce == true)
        {
            _FlipBoss(bRight);
            m_bBossFlipRight = bRight;
        }
        else
        {
            if (bRight != m_bBossFlipRight && m_bDashToggle == false)
            {
                _FlipBoss(bRight);
                m_bBossFlipRight = bRight;
            }
        }
    }
    private void FixedUpdate()
    {
        _processFlipBoss(false);
    }
}
