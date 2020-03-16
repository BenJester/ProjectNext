using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertGravity : Skill
{
    // Start is called before the first frame update
    void Start()
    {
        playerControl.swap.OnSwap += HandleSwap;
    }
    void HandleSwap()
    {
        foreach (var thing in playerControl.thingList)
        {
            thing.body.gravityScale *= -1;
        }
        //playerControl.rb.gravityScale *= -1;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
