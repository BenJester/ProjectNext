using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject bullet;
    public float speed;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(Vector2 dir) {
        GameObject bulletIns =  Instantiate(bullet, (Vector2)transform.position+dir*70f, Quaternion.identity);
        bulletIns.GetComponent<Rigidbody2D>().velocity = dir * speed;
    }
}
