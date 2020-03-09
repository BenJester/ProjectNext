using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_BulletShooter : Mech_base
{
    // Start is called before the first frame update

    public SpriteRenderer spriteR;
    public GameObject bulletToShoot;
    public float coolDown = 1f;
    public float bulletSpeed = 20f;
    public Transform spawnPoint;

    [HideInInspector]
    public Enemy enemyInstance;

    private float timeTemp;

    void Start()
    {
        base.Start();
        timeTemp = Time.time;
        if(spriteR!=null) spriteR.sprite=bulletToShoot.GetComponent<SpriteRenderer>().sprite;
        if (spawnPoint == null) spawnPoint = transform;
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
        GameObject bullet =  Instantiate(bulletToShoot, spawnPoint.position, Quaternion.identity) as GameObject;
        if (bullet.GetComponent<Enemy>() != null)
        {
            enemyInstance = bullet.GetComponent<Enemy>();
        }
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
