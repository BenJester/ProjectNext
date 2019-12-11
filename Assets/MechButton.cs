using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechButton : MonoBehaviour
{
    public bool onlyOnce;
    public bool isPressed=false;
    public Mech_base mech;
    public LineRenderer lr;
    private Vector3 oriPos;
    private float cd;

    void Start()
    {        
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, mech.transform.position);
        oriPos = transform.position;
    }

    
    void Update()
    {
        if (isPressed && mech.type==Mech_base.MechType.Doing)
        {
            mech.Doing();
        }

        if (cd > 0)
        {
            cd -= Time.deltaTime;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((!isPressed && collision.tag == "thing") || (!isPressed && collision.tag =="player"))
        {
            isPressed = true;
            transform.position = oriPos - new Vector3(0, 10,0);

            if (cd <= 0)
            {
                mech.DoOnce();
                if (onlyOnce)
                    Destroy(gameObject);
                cd = 1;
            }
        }        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if((isPressed && collision.tag == "thing") || (isPressed && collision.tag == "player")){
            isPressed = false;
            transform.position = oriPos;
        }
    }
}
