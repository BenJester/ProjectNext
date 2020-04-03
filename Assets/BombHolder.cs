using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombHolder : MonoBehaviour
{
    // Start is called before the first frame update
    public Ti_TimerBomb bomb;
    LineRenderer lineRenderer;
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
        if(other.tag == "thing" && other.GetComponent<EnemyBullet_Transable_Forward>()!=null){
            bomb.Fall();
            lineRenderer.enabled = false;
        }
    }
}
