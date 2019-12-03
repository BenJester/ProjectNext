using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ben;
public class Enemy_TouchDamage : MonoBehaviour
{
    public bool EnemyDamage;
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
        if( collision.CompareTag(GlobalTagDefine.TagName_player) == true )
        {
            PlayerControl1 _ctrl = collision.GetComponent<PlayerControl1>();
            if( _ctrl != null )
            {
                _ctrl.GetComponent<Thing>().Die();
                StartCoroutine(_ctrl.DelayRestart());
            }
        }
        else if (EnemyDamage == true && collision.gameObject.CompareTag("thing"))
        {
            Thing colThing = collision.gameObject.GetComponent<Thing>();
            if(colThing != null)
            {
                if (colThing.type == Type.enemy)
                {
                    colThing.GetComponent<Enemy>().TakeDamage(1);
                }
            }
        }

    }
}
