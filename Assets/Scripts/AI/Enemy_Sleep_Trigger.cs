using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sleep_Trigger : MonoBehaviour
{
    public Enemy_Sleep sleepAI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != gameObject)
        {
            sleepAI.WakeUp();
        }
    }
}
