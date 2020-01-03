using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillShoot : EnemySkillBase
{
    public GameObject ObjShoot;
    public float objInstanceDistance;
    public float objSpeed;
    public bool IsLocal;
    public bool IsHorizontal;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Registe(this);
    }
    public override void CastSkill()
    {
        base.CastSkill();


        Vector3 direction;
        if(IsHorizontal == false)
        {
            direction = (m_transPlayer.position - transform.position).normalized;
        }
        else
        {
            direction = transform.right;
        }
        GameObject newBullet = Instantiate(ObjShoot, transform.position + objInstanceDistance * (Vector3)direction, Quaternion.identity);
        if (IsLocal == true)
        {
            newBullet.transform.SetParent(transform);
            newBullet.transform.localPosition = new Vector3();
            newBullet.transform.localRotation = Quaternion.AngleAxis(0, Vector2.up);
        }
        Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();
        if(bulletBody != null)
        {
            bulletBody.velocity = direction * objSpeed;
        }
        else
        {
            Debug.Assert(false, "没有找到rigidbody");
        }

    }
}
