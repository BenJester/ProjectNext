using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergyCatch : MonoBehaviour
{
    public GameObject starwberryParticle;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetstarberryParticle()
    {
        GameObject g = Instantiate(starwberryParticle, transform.position, Quaternion.identity);
        g.transform.SetParent(transform);
        Destroy(g, 2f);
    }
}
