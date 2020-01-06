using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillTracingPlayer : EnemySkillBase
{
    public float SpeedOfEnemy;
    // Start is called before the first frame update
    private PlayerControl1 m_player;
    private Rigidbody2D m_rigidbody;
    protected override void Start()
    {
        base.Start();
        Registe(this);
        m_player = PlayerControl1.Instance;
        m_rigidbody = GetComponent<Rigidbody2D>();
    }
    public override void CastSkill()
    {
        base.CastSkill();
    }

    private void FixedUpdate()
    {
        if( IsSkillCasting() )
        {
            Vector2 vecDir = m_player.transform.position - transform.position;
            m_rigidbody.MovePosition(new Vector2(transform.position.x, transform.position.y) + vecDir * SpeedOfEnemy * Time.fixedDeltaTime);
        }
    }
}