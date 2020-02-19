using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowKunai : MonoBehaviour
{
    // Start is called before the first frame update
    public Kunai kunai1;
    public Kunai kunai2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            kunai1.HandleInput((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized);
        }
        if (Input.GetMouseButtonUp(1))
        {
            kunai2.HandleInput((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized);
        }
    }
}
