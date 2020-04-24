using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLaserHelper : MonoBehaviour
{
    public MechTriggerArea triggerArea;
    public MovingLaser laser;
    public bool off;
    public float activeInterval;
    public float inactiveInterval;
    void Awake()
    {
        laser.triggerArea = triggerArea;
        if (off)
        {
            laser.isIntervalsLaser = false;
            laser.isDoubleIntervalLaser = false;
            laser.active = false;
        }
        if (activeInterval != 0f)
        {
            laser.interval = activeInterval;
        }
        if (inactiveInterval != 0f)
        {
            laser.inactiveInterval = inactiveInterval;
            laser.isDoubleIntervalLaser = true;
            laser.isIntervalsLaser = false;
        }
            
    }

    void Update()
    {
        
    }

    public void SetOff(bool isOff) {
        off = isOff;
    }
}
