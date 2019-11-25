using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable]
public class MyThingUnityEvent : UnityEvent<Thing>
{
}
[System.Serializable]
public class MyPlayerUnityEvent : UnityEvent<PlayerControl1>
{
}
public class EnemySkillMeleeAttack : EnemySkillBase
{
    public GameObject MeleeGameObject;
    public Vector3 OffsetWithParent;

    public UnityEvent<Thing> ua;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Registe(this);
    }
    public override void CastSkill()
    {
        base.CastSkill();

        GameObject _insMelee = Instantiate(MeleeGameObject, Vector3.zero, Quaternion.identity);
        _insMelee.transform.SetParent(transform);
        _insMelee.transform.localPosition = OffsetWithParent;
    } 
}