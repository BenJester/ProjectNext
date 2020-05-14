using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ti_Pickup_Energy : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject particle;
    AudioSource asr;
    void Start()
    {
        asr = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "player")
        {
            Energy.Instance.energy = Energy.Instance.maxEnergy;
            Energy.Instance.freeSwap = true;
            asr.Play();
            GameObject part1 = Instantiate(particle, transform.position, Quaternion.identity);
            Destroy(part1, 1f);
            GetComponent<Thing>().Die();

        }
    }

}
