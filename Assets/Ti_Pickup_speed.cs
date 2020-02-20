using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ti_Pickup_speed : TriggerItem_Base
{
    // Start is called before the first frame update
    public int maxspeedup=100;
    public GameObject triggerEffect;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void HandleKickTrigger(){
        
        PlayerControl1.Instance.speed+=maxspeedup;
    
        Destroy(gameObject);

    }

    public override void HandleSwapTrigger(){
       PlayerControl1.Instance.speed+=maxspeedup;
    
        Destroy(gameObject);
    }

   
    void OnDestroy()
    {
        GameObject Object=Instantiate(triggerEffect,transform.position,Quaternion.identity);
        Object.transform.parent=null;
    }

}
