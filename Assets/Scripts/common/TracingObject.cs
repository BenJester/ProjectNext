using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracingObject : MonoBehaviour
{
    public float TracingSpeed;
    
    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, GlobalVariable.GetPlayer().transform.position, TracingSpeed * Time.fixedDeltaTime);
    }
}
