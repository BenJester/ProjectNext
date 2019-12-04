using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_BulletShooter : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject bulletToShoot;
    public float coolDown = 1f;
    public float bulletSpeed = 20f;

    private float timeTemp;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TODO:要考虑到整体的子弹时间到底要怎么写老哥！
        if(timeTemp<Time.time){
            Shoot();
            timeTemp+=coolDown;
        }
    }

    void Shoot(){
        GameObject bullet =  Instantiate(bulletToShoot,(Vector2)transform.position+Vector2.right*10,Quaternion.identity) as GameObject;
        bullet.GetComponent<Rigidbody2D>().velocity = transform.up*bulletSpeed;
    }
}
