using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Patrol_Collider : MonoBehaviour
{
    public bool active;
    public Enemy_Patrol self;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!active) return;
        if (col.CompareTag("player"))
        {
            Debug.Log("hakken");
            HandleDetection(col.gameObject);
        }
    }
    private void Update()
    {
        if (self.thing.dead)
        {
            active = false;
            self.colSr.enabled = false;
        }
            
    }
    void HandleDetection(GameObject pos)
    {
        self.Detect(pos);
    }
}
