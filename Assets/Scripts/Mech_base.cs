using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_base : MonoBehaviour
{

    public enum MechType
    {
        update=0,
        Once=1,
        Doing=2,
    }
    public MechType type;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void DoOnce()
    {

    }
    public virtual void Doing()
    {

    }
}
