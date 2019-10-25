using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTestCanvas : MonoBehaviour
{

    public static LevelTestCanvas instance;
    private void Awake()
    {
        if (instance)
            Destroy(gameObject);
        if (!instance) instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
