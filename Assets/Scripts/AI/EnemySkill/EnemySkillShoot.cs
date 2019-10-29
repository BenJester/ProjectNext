using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillShoot : EnemySkillBase
{
    public GameObject ObjShoot;
    public float objInstanceDistance;
    public float objSpeed;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    public override void CastSkill()
    {
        base.CastSkill();


        Vector3 direction = (m_transPlayer.position - transform.position).normalized;
        GameObject newBullet = Instantiate(ObjShoot, transform.position + objInstanceDistance * (Vector3)direction, Quaternion.identity);
        Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();
        bulletBody.velocity = direction * objSpeed;
    }
}
