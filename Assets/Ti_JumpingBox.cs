using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ti_JumpingBox : MonoBehaviour
{
    public float force;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddForceToPlayer() {
        PlayerControl1.Instance.GetComponent<Rigidbody2D>().AddForce(Vector2.up * force);
    }
    public void AddForceToSelf()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * force);
    }
}
