using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area_EnergyFull : MonoBehaviour
{

    AudioSource asr;
    public GameObject particle;
   
    // Start is called before the first frame update
    void Start()
    {
        asr = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player" && particle!=null)
        {
            asr.Play();
            GameObject part1 = Instantiate(particle, transform.position, Quaternion.identity);
            Destroy(part1, 1f);


        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            Energy.Instance.energy = Energy.Instance.maxEnergy;
            Energy.Instance.freeSwap = true;



        }
    }
}
