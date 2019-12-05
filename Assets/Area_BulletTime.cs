using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area_BulletTime : MonoBehaviour
{

    private BulletTime bulletTime;
    // Start is called before the first frame update


    private void Awake()
    {
        bulletTime = GameObject.FindGameObjectWithTag("player").GetComponent<BulletTime>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            bulletTime.ActiveBulletTime( true, BulletTime.BulletTimePriority.BulletTimePriority_High);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            bulletTime.ActiveBulletTime(false, BulletTime.BulletTimePriority.BulletTimePriority_High);
        }
    }
}
