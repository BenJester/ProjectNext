using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLaserHelper : MonoBehaviour
{
    public MechTriggerArea triggerArea;
    public MovingLaser laser;
    public bool off;
    void Awake()
    {
        laser.triggerArea = triggerArea;
        if (off)
        {
            laser.isIntervalsLaser = false;
            laser.isDoubleIntervalLaser = false;
            laser.active = false;
        }
            
    }

    void Update()
    {
        
    }
}
