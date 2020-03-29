using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikeland_Trigger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject spike;
    [Header("勾选后就是等待玩家完全离开后出现")]
    public bool totalDelay;
    public float delayTime;
    public bool foreverTrigger;
    public float dispearTime;
    bool isTrigger = false;
    public Color triggerColor;
    float temp;
    public SpriteRenderer spr;
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    //这个可能写的会有Bug，就是说玩家在消失的时候跳入？
    // Update is called once per frame
    void Update()
    {
        if(isTrigger && Time.time>temp){

            isTrigger = true;
            spr.color = triggerColor;
            StartCoroutine(SetSpike());

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTrigger &&!totalDelay && collision.tag == "player") { 
            isTrigger = true;
            temp = Time.time+delayTime;

        
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //完全等待玩家移动之后再出现，更加谨慎
        if (totalDelay && collision.tag == "player") {
            spike.SetActive(true);


        }    
    }

    IEnumerator SetSpike(){
        
        spike.SetActive(true);
        spike.GetComponent<SpriteRenderer>().color= Color.black;
        if(!foreverTrigger){
            yield return new WaitForSeconds(dispearTime);
            spike.SetActive(false);
            spike.GetComponent<SpriteRenderer>().color= Color.white;
            isTrigger = false;
            spr.color = Color.white;
        }


    }
}
