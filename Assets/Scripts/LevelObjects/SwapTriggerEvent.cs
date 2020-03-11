using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwapTriggerEvent : MonoBehaviour
{
    public UnityEvent TriggerEvent;

    void Start()
    {
        PlayerControl1.Instance.swap.OnSwap += HandleSwap;
    }
    void HandleSwap()
    {
        TriggerEvent?.Invoke();
    }

    void Update()
    {
        
    }
}
