using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  EnergyIndicator : MonoBehaviour
{

    public static EnergyIndicator instance;

    public GameObject energyParticle;
    private GameObject _energyParticle;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    void Start()
    {
        RespawnEnergyParticle();
    }

    
    void Update()
    {
        
    }

    public void RespawnEnergyParticle()
    {
        _energyParticle = Instantiate(energyParticle, transform.position + new Vector3(0,8,0), transform.rotation, transform) ;
    }

    public void CloseEnergyParticle()
    {
        _energyParticle.SetActive(false);
        //Debug.Log("setfalse energy");
    }
    public void TransferEnergyParticle(Transform target)
    {
        
        _energyParticle.transform.position = target.transform.position;
        _energyParticle.SetActive(true);
        _energyParticle.transform.SetParent(target);
        Destroy(_energyParticle, 1f);
    }

    //玩家自己出现力量
   
}
