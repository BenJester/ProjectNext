using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//依据玩家位置投掷道具
public class EnemySkillThrowBomb : EnemySkillBase
{
    [Header("依据玩家位置投掷道具")]
    [Tooltip("道具prefab")]
    public GameObject ObjShoot;
    [Tooltip("投掷距离")]
    public float objInstanceDistance;
    [Tooltip("投掷高度")]
    public float objInstanceUpDistance;
    [Tooltip("投掷速度")]
    public float objSpeed;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Registe(this);
    }

    public override void CastSkill()
    {
        base.CastSkill();
        Vector3 direction = (Vector3.up * 2f + (m_transPlayer.position - transform.position).normalized).normalized;
        GameObject newBullet = Instantiate(ObjShoot, new Vector3(transform.position.x, transform.position.y + objInstanceUpDistance, transform.position.z) + objInstanceDistance * (Vector3)direction, Quaternion.identity);
        Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();
        bulletBody.velocity = direction * objSpeed;
    }
}
