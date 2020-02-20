using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ti_Pickup_Health : TriggerItem_Base
{
    // Start is called before the first frame update
    public int maxhpUp=1;
    public int hpRecover=1;
    public GameObject triggerEffect;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void HandleKickTrigger(){
        
        PlayerControl1.Instance.maxhp+=maxhpUp;
        PlayerControl1.Instance.hp+=hpRecover;
        Destroy(gameObject);

    }

    public override void HandleSwapTrigger(){
        PlayerControl1.Instance.maxhp+=maxhpUp;
        PlayerControl1.Instance.hp+=hpRecover;
        Destroy(gameObject);
    }

   
    void OnDestroy()
    {
        GameObject Object=Instantiate(triggerEffect,transform.position,Quaternion.identity);
        Object.transform.parent=null;
        Destroy(Object,1f);
    }

}
