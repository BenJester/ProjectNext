using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyIndicator : MonoBehaviour
{

    public GameObject energyParticle;
    private GameObject _energyParticle;
    
    void Start()
    {
        RespawnEnergyParticle();
    }

    
    void Update()
    {
        
    }

    public void RespawnEnergyParticle()
    {
        _energyParticle = Instantiate(energyParticle, transform.position, transform.rotation, transform);
    }
    public void TransferEnergyParticle(Transform target)
    {
        _energyParticle.transform.SetParent(target);
    }
}
