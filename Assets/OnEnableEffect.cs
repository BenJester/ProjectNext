using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableEffect : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject particleEffect;
    public Vector2 offset;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if (particleEffect != null) {
            GameObject part = Instantiate(particleEffect, (Vector2)transform.position + offset, Quaternion.identity);
            Destroy(part, 1.5f);
        }
    }
}
