using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameGroup : MonoBehaviour
{
    // Start is called before the first frame update
    public Flame[] flames;

    private void Awake()
    {
        flames = transform.GetComponentsInChildren<Flame>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
