using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ti_Pickup_Health : TriggerItem_Base
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void HandleKickTrigger(){
        for (int i = 0; i < count; i++)
        {
            GameObject Object= Instantiate(instance,transform.position,Quaternion.identity);
            Object.transform.parent=null;
        }
        Destroy(gameObject);

    }

    public override void HandleSwapTrigger(){
        for (int i = 0; i < count; i++)
        {
            GameObject Object= Instantiate(instance,transform.position,Quaternion.identity);
            Object.transform.parent=null;
        }
        Destroy(gameObject);
    }

}
