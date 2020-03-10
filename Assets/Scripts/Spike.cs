using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ben;
//嗨，spike是要求又boxcollider2d处理trigger为true的，如果你把boxcollider2d删除，请确认，添加上的collider.istrigger = true
[RequireComponent(typeof(CompositeCollider2D))]
public class Spike : MonoBehaviour {
    public bool destroyBox = false;
	// Use this for initialization
	void Start () {
        CompositeCollider2D _composite = GetComponent<CompositeCollider2D>();
        if( _composite != null )
        {
            //因为地图上可能会有一些留存spike没有改成trigger。所以写一个脚本来保证每个spike都是trigger
            _composite.isTrigger = true;
        }
        else
        {
            BoxCollider2D _boxCollider = GetComponent<BoxCollider2D>();
            if(_boxCollider != null )
            {
                _boxCollider.isTrigger = true;
            }
            else
            {
                Debug.Assert(false, "这个刺的collider是不是又被改掉拉？");
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//void OnCollisionEnter2D(Collision2D col) {
	//	if (col.gameObject.CompareTag("thing") || col.gameObject.CompareTag("player")) {
	//		Thing colThing = col.gameObject.GetComponent<Thing> ();
	//		if (colThing.type == Type.enemy) {
 //               colThing.GetComponent<Enemy>().TakeDamage(1);
	//		}
 //           if (colThing.type == Type.player)
 //           {
 //               colThing.Die();
 //               StartCoroutine(colThing.GetComponent<PlayerControl1>().DelayRestart());
 //           }
	//	}
	//}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("thing") || collision.gameObject.CompareTag("player"))
        {
            Thing colThing = collision.gameObject.GetComponent<Thing>();

            if (colThing.GetComponent<Enemy>() != null)
            {
                colThing.GetComponent<Enemy>().TakeDamage(1);
            }
            if (colThing.type == Type.player)
            {
                colThing.GetComponent<PlayerControl1>().Die();
                //StartCoroutine(colThing.GetComponent<PlayerControl1>().DelayRestart());
            }
            if (colThing.type == Type.box)
            {
                collision.GetComponent<Box>().GetSpike();
                if (destroyBox)
                {
                    colThing.Die();
                }
                    
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("thing"))
        {
            Thing colThing = collision.gameObject.GetComponent<Thing>();
            if (colThing.type == Type.box)
            {
                collision.GetComponent<Box>().OutSpike();
            }
        }
    }
}
