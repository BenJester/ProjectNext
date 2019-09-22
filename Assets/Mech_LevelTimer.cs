using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Mech_LevelTimer : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMesh timer1;
    public TextMesh timer2;
    public Text levelTimer;

    public float timer1Time=0;
    public float timer2Time=0;

    public bool timer1Counting=false;
    public bool timer2Counting=false;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer1Counting)
        {
            timer1Time += Time.deltaTime;
            float y = timer1Time;
            string text = string.Format("{0:0.00}", y);
            timer1.text = text;
            levelTimer.text = text +"s";
        }

        if (timer2Counting)
        {
            timer2Time += Time.deltaTime;
            float y = timer2Time;
            string text = string.Format("{0:0.00}", y);
            timer2.text = text;
        }

    }

    public void SetTimer(int timerID,float time)
    {
        if (timerID == 1) timer1.text = 0.00f.ToString();
        if (timerID == 2) timer2.text = 0.00f.ToString();
    }

    public void StartTimer(int timerID)
    {
        if (timerID == 1) timer1Counting=true;
        if (timerID == 2) timer2Counting =true;
    }

    public void StopTimer(int timerID)
    {
        if (timerID == 1) timer1Counting = false;
        if (timerID == 2) timer2Counting = false;

    }

    public bool GetTimerCounting(int timerID)
    {
        if (timerID == 1) return timer1Counting;
        else return timer2Counting;
    }

    
}
