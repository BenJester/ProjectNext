using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillFaceToPlayer : EnemySkillBase
{
    // Start is called before the first frame update
    private Transform player;
    protected override void Start()
    {
        base.Start();
        Registe(this);
        player = GameObject.FindGameObjectWithTag("player").transform;
    }
    public override void CastSkill()
    {
        base.CastSkill();

    }
    private void FixedUpdate()
    {
        if (IsSkillCasting())
        {
            if (player.transform.position.x > transform.position.x)
                transform.eulerAngles = new Vector3(0, 0, 0);
            else
                transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
}