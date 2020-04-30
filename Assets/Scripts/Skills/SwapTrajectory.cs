using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapTrajectory : MonoBehaviour
{
    public bool active;
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Flame") && active)
        {
            col.GetComponent<Flame>().Activate();
        }
    }


    void Update()
    {
        
    }
}
