using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Ben;
public class MeleeAttack : MonoBehaviour
{
    public int DamageOfMelee;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(GlobalTagDefine.TagName_player))
        {
            PlayerControl1 _player = collision.GetComponent<PlayerControl1>();
            if (_player != null)
            {
                _player.Die();
            }
            else
            {
                Debug.Assert(false);
            }
        }
        else if( collision.CompareTag(GlobalTagDefine.TagName_thing))
        {
            Thing _thing = collision.GetComponent<Thing>();
            if (_thing != null)
            {
                if(_thing.type == Type.enemy)
                {
                    Enemy _enemy = _thing.GetComponent<Enemy>();
                    if(_enemy != null)
                    {
                        _enemy.TakeDamage(DamageOfMelee);
                    }
                    else
                    {
                        Debug.Assert(false, string.Format("近战攻击到了一个没有enemy组件的敌人"));
                    }
                }
                else
                {
                    Debug.Assert(false, string.Format("非enemy该如何判断？"));
                }
            }
            else
            {
                Debug.Assert(false,string.Format("没有thing的物件"));
            }
        }
    }
}
