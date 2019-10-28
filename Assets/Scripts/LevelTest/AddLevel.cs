using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AddLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Tilemap[] tilemaps = GetComponentsInChildren<Tilemap>();
        for (int i = 0; i < tilemaps.Length; i++)
        {
            tilemaps[i].gameObject.AddComponent<Level>();
        }
    }
}
