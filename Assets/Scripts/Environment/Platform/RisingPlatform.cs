using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingPlatform : MonoBehaviour
{
    Rigidbody2D rb;
    bool rising = false;
    public float riseSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (rising)
            rb.velocity = new Vector2(0f, riseSpeed);
    }

    public void Rise()
    {
        rising = true;
    }
}
