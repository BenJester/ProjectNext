using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  EnergyIndicator : MonoBehaviour
{

    public static EnergyIndicator instance = null;

    public GameObject energyParticle;
    private GameObject _energyParticle;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
        }
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
        if(_energyParticle != null)
        {
            _energyParticle.SetActive(false);
        }
        else
        {
        }
        //Debug.Log("setfalse energy");
    }
    public void TransferEnergyParticle(Transform target)
    {
        if(_energyParticle != null)
        {
            _energyParticle.transform.position = target.transform.position;
            _energyParticle.SetActive(true);
            _energyParticle.transform.SetParent(target);
            Destroy(_energyParticle, 1f);
        }
    }

    public void DestroyParticle()
    {
        Destroy(_energyParticle, 1f);
    }

    //玩家自己出现力量
   
}
