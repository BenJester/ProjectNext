using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWorldManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetWm() {
        
        AudioSource asr = GameObject.FindGameObjectWithTag("WorldManager").GetComponent<AudioSource>();
        if (asr.isPlaying != true) {
            asr.Play();
        }
    }
}
