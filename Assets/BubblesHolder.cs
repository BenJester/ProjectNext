using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BubblesHolder : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] Bubbles;
    public bool[] bubblesState;


    public UnityEvent trigger;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Check()) trigger.Invoke();


    }

    bool Check() {
        foreach (var item in bubblesState)
        {
            if (item == true) 
                return false;
        }
        return true;
    
    
    }
}
