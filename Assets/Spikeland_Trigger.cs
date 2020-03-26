using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikeland_Trigger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject spike;
    public bool totalDelay;
    public bool delayTIme;
    public bool foreverTrigger;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!totalDelay && collision.tag == "player") { 
        
        
        
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //完全等待玩家移动之后再出现，更加谨慎
        if (totalDelay && collision.tag == "player") {
            spike.SetActive(true);


        }    
    }
}
