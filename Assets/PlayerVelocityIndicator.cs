using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVelocityIndicator : MonoBehaviour
{
    // Start is called before the first frame update
    public LineRenderer LR;
    void Start()
    {
        LR= GetComponent<LineRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        LR.SetPosition(0,transform.position);
        LR.SetPosition(1,(Vector2)transform.position+PlayerControl1.Instance.GetComponent<Rigidbody2D>().velocity.normalized*100);
    }
}
