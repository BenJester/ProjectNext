using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medicine : MonoBehaviour
{
    // Start is called before the first frame update

     LineRenderer lr;
    public float radius;
    public GameObject indicator;
    float startTime;
    public float medineTime;

    public bool StartMec = false;
    public PlayerControl1 pc;
    void Start()
    {
        pc = PlayerControl1.Instance;
        lr = GetComponent<LineRenderer>();
        //GameObject area = Instantiate(indicator, transform.position, Quaternion.identity);
        //area.transform.parent = null;
        //area.GetComponent<SpriteRenderer>().size = new Vector2(radius * 2, radius * 2);
        //area.GetComponent<SpriteRenderer>().color = Color.green;
        //area.transform.parent = transform;
        
    }

    
    void Update()
    {
        if (CheckPlayer())
        {
            if (!StartMec)
            {
                StartMec = true;
                startTime = Time.time;
                lr.enabled = true;
            }

            if (Time.time - startTime >= medineTime )
            {
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, pc.transform.position);
                StartMedicine();
            }

        }
        else {
            StartMec = false;
            lr.enabled = false;
        }
    }

    bool CheckPlayer() {
        return Vector2.Distance(pc.transform.position, transform.position) <= radius; 
    
    }

    void  StartMedicine() {
       
            pc.hp += 1;
            pc.hp = Mathf.Clamp(pc.hp, 0, pc.maxhp);
        
       
    }
}
