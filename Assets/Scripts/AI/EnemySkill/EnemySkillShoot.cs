using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//飞行道具释放
public class EnemySkillShoot : EnemySkillBase
{
    [Header("飞行道具释放")]
    [Tooltip("释放的prefab")]
    public GameObject ObjShoot;
    [Tooltip("释放朝前距离")]
    public float objInstanceDistance;
    [Tooltip("释放飞行速度")]
    public float objSpeed;
    [Tooltip("如果是local的话就是相对于本身transform位置")]
    public bool IsLocal;
    [Tooltip("不是水平移动就是根据玩家位置进行")]
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
