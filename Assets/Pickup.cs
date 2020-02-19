using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : TriggerItem_Base
{
    // Start is called before the first frame update
    public GameObject instance;
    public int count=1;
    public int level=0;
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

    }

    
}
