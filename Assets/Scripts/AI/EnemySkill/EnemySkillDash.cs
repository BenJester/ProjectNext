using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillDash : EnemySkillBase
{
    public bool OnlyHorizontalDash;
    public float dashSpeed;
    public float DashHitOffsetY;
    public float hitboxWidth;
    public float DashDuration;

    private Vector3 m_vecDashDir;
    private Rigidbody2D m_rigidBody;
    private BoxCollider2D m_ColliderBox;
    private BoxCollider2D m_ColliderPlayerBox;

    private float m_fCurrentTime;
    private PlayerControl1 m_playerCtrl;

    private Enemy_FlipByPlayer m_flipEnemy;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Registe(this);
        m_flipEnemy = GetComponent<Enemy_FlipByPlayer>();
        m_rigidBody = GetComponent<Rigidbody2D>();

        m_ColliderPlayerBox = m_transPlayer.GetComponent<BoxCollider2D>();
        m_ColliderBox = GetComponent<BoxCollider2D>();

        m_playerCtrl = m_transPlayer.GetComponent<PlayerControl1>();
    }
    private void Update()
    {
        if(IsSkillCasting() == true)
        {
            if (m_fCurrentTime >= DashDuration)
            {
                SetSkillCastring(false);
                m_rigidBody.velocity = Vector2.zero;
                if(m_flipEnemy != null)
                {
                    m_flipEnemy.UpdateValid(true);
                }
            }
            else
            {
                Vector3 vecSource = new Vector2(transform.position.x, transform.position.y - DashHitOffsetY);
                Physics2D.IgnoreCollision(m_ColliderBox, m_ColliderPlayerBox, true);
                m_rigidBody.velocity = m_vecDashDir * dashSpeed;
                float fAngle = Vector2.SignedAngle(transform.position, m_transPlayer.position);
                Collider2D[] cols = Physics2D.OverlapBoxAll(vecSource + m_vecDashDir * (m_ColliderBox.size.x + hitboxWidth),
                                 new Vector2(hitboxWidth * 2, m_ColliderBox.size.y),
                                 fAngle);

                foreach (Collider2D col in cols)
                {
                    if (col.CompareTag("player"))
                    {
                        m_playerCtrl.Die();
                    }
                }
                Physics2D.IgnoreCollision(m_ColliderBox, m_ColliderPlayerBox, false);

                m_fCurrentTime += Time.deltaTime;
            }
        }
    }
    public override void CastSkill()
    {
        base.CastSkill();
        if (OnlyHorizontalDash == true)
        {
            m_vecDashDir = (new Vector3(m_transPlayer.position.x, transform.position.y, m_transPlayer.position.z) - transform.position).normalized;
        }
        else
        {
            m_vecDashDir = (m_transPlayer.position - transform.position).normalized;
        }
        m_fCurrentTime = 0.0f;
        if (m_flipEnemy != null)
        {
            m_flipEnemy.UpdateValid(false);
        }
    }
}
