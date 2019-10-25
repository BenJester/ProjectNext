using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDestroyPlatform : MonoBehaviour
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
        if (collision.gameObject.CompareTag(GlobalTagDefine.TagName_player))
        {
            Thing colThing = collision.gameObject.GetComponent<Thing>();
            if(colThing != null)
            {
                if (colThing.type == Type.player)
                {
                    colThing.Die();
                    StartCoroutine(colThing.GetComponent<PlayerControl1>().DelayRestart());
                }
            }
            else
            {
                Debug.Assert(false);
            }
        }
        else if (collision.gameObject.CompareTag(GlobalTagDefine.TagName_thing))
        {
            Thing colThing = collision.gameObject.GetComponent<Thing>();
            if (colThing != null && colThing.type == Type.enemy)
            {
                colThing.GetComponent<Enemy>().TakeDamage(1);
                SpawnObjAfterDestroy _spawnObj = GetComponent<SpawnObjAfterDestroy>();
                if (_spawnObj != null)
                {
                    _spawnObj.SpawnObject();
                }
                Destroy(gameObject);
            }
            else
            {
                if( colThing == null )
                {
                    Debug.Assert(false);
                }
            }
        }
    }
}
