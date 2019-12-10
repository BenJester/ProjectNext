using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_BulletShooter : Mech_base
{
    // Start is called before the first frame update



    public GameObject bulletToShoot;
    public float coolDown = 1f;
    public float bulletSpeed = 20f;

    private float timeTemp;

    void Start()
    {
        timeTemp = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(type==MechType.update && timeTemp<Time.time){
            Shoot();
            timeTemp+=coolDown;
        }
    }

    void Shoot(){
        GameObject bullet =  Instantiate(bulletToShoot,(Vector2)transform.position+Vector2.right*10,Quaternion.identity) as GameObject;
        bullet.GetComponent<Rigidbody2D>().velocity = transform.up*bulletSpeed;
    }

    public override void DoOnce()
    {
        base.DoOnce();
        Shoot();
    }

    public override void Doing()
    {
        base.Doing();
    }
}
