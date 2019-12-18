using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelfDestroyNotify : MonoBehaviour
{
    public UnityEvent DestroyDedicator;
    // Start is called before the first frame update
    private void OnDestroy()
    {
        if(DestroyDedicator != null)
        {
            DestroyDedicator.Invoke();
        }
    }
}
