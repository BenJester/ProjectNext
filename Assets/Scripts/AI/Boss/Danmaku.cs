using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danmaku : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bullet;
    public GameObject bigBullet;
    public GameObject hpPack;
    public GameObject atkUpPack;

    public bool shootAtOnce;
    public int shootNum;

    public Transform start;
    public Transform finish;
    public float xSpeed;
    public float ySpeed;

    public void Shoot()
    {
        Vector3 currPos = start.position;
        Vector3 interval = (finish.position - start.position) / (float) shootNum;
        for (int i = 0; i < shootNum; i ++)
        {
            GameObject bulletObj = Instantiate(bullet, currPos, Quaternion.identity);
            Rigidbody2D bulletBody = bulletObj.GetComponent<Rigidbody2D>();
            bulletBody.velocity = new Vector2(xSpeed, ySpeed);
            currPos += interval;
        }
    }

    void Start()
    {
        Shoot();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
