using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombHolder : MonoBehaviour
{
    // Start is called before the first frame update
    public Thing thing;
    LineRenderer lineRenderer;
    //public bool cancleShield = true;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0,transform.parent.transform.position);
        lineRenderer.SetPosition (1, new Vector2(GetComponent<BoxCollider2D>().bounds.center.x,GetComponent<BoxCollider2D>().bounds.center.y+GetComponent<BoxCollider2D>().bounds.size.y/2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "thing" && (other.GetComponent<EnemyBullet_Transable_Forward>()!=null || other.GetComponent<Dir_spear>()!=null)){
            thing.Fall();
            lineRenderer.enabled = false;
            if(thing.cancleShieldWhenRopeCut)    thing.hasShield = false;
        }
    }
}
