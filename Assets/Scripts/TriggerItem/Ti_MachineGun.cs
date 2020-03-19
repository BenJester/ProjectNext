using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ti_MachineGun : Ti_GunType,TriggerItem_Base
{
    // Start is called before the first frame update

    public bool floatGun=false;
    public bool isRight = true;
    Vector2 direction;
    public GameObject bullet;
    public int times;
    public float intervals;
    public float recoil;
    public float bulletSpeed;
    public float bulletInstanceDistance = 50f;




    void Start()
    {
        base.Start();
        if (!isRight)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HandleKickTrigger()
    {
        StartCoroutine(Shoot());
    }

    public void HandleSwapTrigger()
    {
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        if (ammoNow <= 0)
        {
            Destroy(gameObject);
        }
        if (floatGun){
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
        //Dir
        if (isRight) direction = transform.right;
        else direction = -transform.right;
        int i = 0;
        yield return new WaitForSeconds(0f);
        while (i < times)
        {
            if (ammoNow > 0)
            {
                GameObject newBullet = Instantiate(bullet, transform.position + bulletInstanceDistance * (Vector3)direction, Quaternion.identity);
                Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();
                bulletBody.velocity = direction * bulletSpeed;
                yield return new WaitForSeconds(intervals);
                transform.Translate(new Vector3(-direction.normalized.x * recoil, 0, 0));

                ammoNow -= 1;
                SetAmmo();
            }
           
            i++;
        }

        if(floatGun){
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }


        //  


    }
}
