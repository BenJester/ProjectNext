using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySkillSwitchThing : EnemySkillBase
{
    private EnemyAttribute enemyAttribute;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Registe(this);
        enemyAttribute = GetComponent<EnemyAttribute>();
    }
    public override void CastSkill()
    {
        base.CastSkill();
        if (enemyAttribute)
        {
            if (enemyAttribute.target)
            {
                Vector2 tempPosition = enemyAttribute.target. transform.position;
                enemyAttribute.target.transform.position = transform.position;
                transform.position = tempPosition;
            }
        }
    } 
}