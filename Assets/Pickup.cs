using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : TriggerItem_Base
{
    // Start is called before the first frame update
    public string pickupName;
    public Text nameText;
    public GameObject instance;
    public int count=1;
    public int level=0;
    public GameObject triggerEffect;

    
    void Start()
    {
        nameText.text=pickupName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void HandleKickTrigger(){
        GameObject Object=Instantiate(triggerEffect,transform.position,Quaternion.identity);
        Object.transform.parent=null;
        for (int i = 0; i < count; i++)
        {
            GameObject pickup= Instantiate(instance,transform.position,Quaternion.identity);
            pickup.transform.parent=null;
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

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        GameObject ObjectSmoke=Instantiate(triggerEffect,transform.position,Quaternion.identity);
        Destroy(ObjectSmoke,1f);
        ObjectSmoke.transform.parent=null;
    }

    
}
