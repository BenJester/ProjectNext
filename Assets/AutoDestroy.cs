using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float destroyTime;
    // Start is called before the first frame update

    public void StartDestroy(float t)
    {
        if (t == 0)
            Destroy(gameObject, destroyTime);
        else
            Destroy(gameObject, t);
    }
}
