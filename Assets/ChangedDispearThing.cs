using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangedDispearThing : Thing,TriggerItem_Base {
    private Vector2 originPos;

    public override void Start () {
        base.Start();
        originPos = transform.position;
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
        if (originPos != (Vector2) transform.position) {
            Destroy(gameObject);
        }
    }

    public void HandleKickTrigger(){

    }

    public void HandleSwapTrigger(){
        
    }
}