using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeLand : MonoBehaviour
{
    // Start is called before the first frame update
    public bool active = false;
    public GameObject spike;
    public float time;
    public float interval;
    public float offset;
    public bool spikeOn=false;

   
    float temp=0;

    
    void Start()
    {
        spike.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active && temp <= Time.time) {
            if (!spikeOn)
            {
                spikeOn = true;
                spike.SetActive(spikeOn);
                temp = Time.time + time;
            }
            else if (spikeOn) {
                spikeOn = false;
                spike.SetActive(spikeOn);
                temp = Time.time + interval;
            }       
        }


        if (!active && Time.time > offset)
        {
            active = true;
        }
    }
}
