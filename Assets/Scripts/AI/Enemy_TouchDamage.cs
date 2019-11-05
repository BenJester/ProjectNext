using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TouchDamage : MonoBehaviour
{
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
                _ctrl.Die();
                StartCoroutine(_ctrl.DelayRestart());
            }
        }

    }
}
