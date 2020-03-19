using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_LevelTimerDoor : MonoBehaviour
{
    // Start is called before the first frame update
    public int thisTimerID;
    public int otherTimerID;
    public Mech_LevelTimer lt;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "player")
        {

            //进入这个起点，清空目标点的记录
            lt.SetTimer(otherTimerID, 0.00f);

            //到达目标点，结束目标点计时
            if (lt.GetTimerCounting(thisTimerID))
            {
                lt.StopTimer(thisTimerID);
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            //开始起跑，目标点开始计时
            lt.StartTimer(otherTimerID);
            lt.StartTimer(thisTimerID);
        }
    }
}
