using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDisappear : MonoBehaviour
{
    [Tooltip("CollisionThing 优先判断")]
    public bool CollisionThing;
    public bool CollisionPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(CollisionThing == true)
        {
            Thing _thing = collision.gameObject.GetComponent<Thing>();
            if(_thing != null)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (CollisionPlayer == true)
            {
                if (collision.gameObject.CompareTag(GlobalTagDefine.TagName_player))
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

}
