using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_LaserTrap : MonoBehaviour
{
    // Start is called before the first frame update

     LineRenderer lr;
    public bool active = true;
    public GameObject point1;
    public GameObject point2;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        if (active)
        {
            lr.SetPosition(0, point1.transform.position);
            lr.SetPosition(1, point2.transform.position);

            Vector2 direction = (point2.transform.position - point1.transform.position).normalized;
            RaycastHit2D[] hits;
            hits = Physics2D.RaycastAll(point1.transform.position,direction,Vector2.Distance(point1.transform.position,point2.transform.position));
            
            foreach(RaycastHit2D hi in hits)
            {
                if(hi.collider.tag == "player")
                {
                    hi.collider.GetComponent<PlayerControl1>().Die();
                }
            }
        }
       
    }

    
}
