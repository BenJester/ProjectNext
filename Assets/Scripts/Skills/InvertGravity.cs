using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertGravity : Skill
{
    // Start is called before the first frame update
    public bool even;
    void Start()
    {
        playerControl.swap.OnSwap += HandleSwap;
    }
    void HandleSwap()
    {
        foreach (var thing in playerControl.thingList)
        {
            thing.body.gravityScale = even ? Mathf.Abs(thing.body.gravityScale) : -Mathf.Abs(thing.body.gravityScale);
        }
        even = !even;
        //playerControl.rb.gravityScale *= -1;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
