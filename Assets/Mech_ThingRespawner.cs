using System.Collections;
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
    public bool hasRespawn=false;

    void Update () {

        if (!hasRespawn) {
            nowEnergy += Time.deltaTime;
            
            if (nowEnergy >= respawnColdDown) {
                respawnedThingInstance = Instantiate (respawnThing, transform.position, Quaternion.identity) as GameObject;
                hasRespawn = true;
                nowEnergy = 0;
            }
        }

        if (respawnedThingInstance !=null && respawnedThingInstance.GetComponent<Thing>().dead)
        {
            hasRespawn = false;
        }
        float fill = (float) nowEnergy / respawnColdDown;
        coolDownSlider.fillAmount = Mathf.Clamp (fill, 0, 1);

        

    }
}