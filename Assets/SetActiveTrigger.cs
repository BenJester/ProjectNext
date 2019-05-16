using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveTrigger : MonoBehaviour {

    public GameObject[] objectToActives;
    public GameObject[] objectToDisactives;

    [Header("协程来PPT触发")]
    private bool timerStart=false;
    private float timer=0;

    [Header("希望触发地时间+0.5f填入（最小0.5f）0")]
    public float[] ActiveTime;
    [Header("与上方时间对应，对应Index触发")]
    public GameObject[] objectToEnumeratorActives;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (timerStart)
        {
            timer += Time.deltaTime;
        }
        if (timerStart && objectToEnumeratorActives.Length>0)
        {
            for (int i = 0; i < ActiveTime.Length; i++)
            {
                if (Mathf.Abs(timer - ActiveTime[i]) < 0.5f)
                {
                    objectToEnumeratorActives[i].SetActive(true);
                }
            }
        }             
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("player"))
        {
            foreach (var objectToActive in objectToActives)
            {
                objectToActive.SetActive(true);
            }

            foreach (var objectToDisable in objectToDisactives)
            {
                objectToDisable.SetActive(false);
            }

            timerStart = true;
        }
        
    }


    
}
