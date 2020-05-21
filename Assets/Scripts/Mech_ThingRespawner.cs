﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mech_ThingRespawner : MonoBehaviour {
    // Start is called before the first frame update

    public GameObject respawnThing;
    public float respawnColdDown;
    public Image coolDownSlider;
    private float nowEnergy;
    private GameObject respawnedThingInstance;
    public bool hasRespawn = false;


    private void Start()
    {
        respawnedThingInstance = Instantiate(respawnThing, transform.position, Quaternion.identity) as GameObject;

        // DestroyNotify _notify = respawnedThingInstance.gameObject.GetComponent<DestroyNotify>();
        // if(_notify != null)
        // {
        //     _notify.RegisteNotifyTarget(_spawnObjectDestroy);
        // }
        hasRespawn = true;
        nowEnergy = 0;
    }
    void Update () {


        _spawnObjectDestroy();
        
        if (!hasRespawn) {
            nowEnergy += Time.deltaTime;
            
            if (nowEnergy >= respawnColdDown) {
                respawnedThingInstance = Instantiate (respawnThing, transform.position, Quaternion.identity) as GameObject;
                
                // DestroyNotify _notify = respawnedThingInstance.gameObject.GetComponent<DestroyNotify>();
                // if(_notify != null)
                // {
                //     _notify.RegisteNotifyTarget(_spawnObjectDestroy);
                // }
                hasRespawn = true;
                nowEnergy = 0;
            }
        }

        float fill = (float) nowEnergy / respawnColdDown;
        coolDownSlider.fillAmount = Mathf.Clamp (fill, 0, 1);
    }

    private void _spawnObjectDestroy()
    {
        if (respawnedThingInstance == null)
        {
            hasRespawn = false;
        }
    }
}