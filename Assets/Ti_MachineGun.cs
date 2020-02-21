using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ti_MachineGun : Ti_GunType
{
    // Start is called before the first frame update


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

    public override void HandleKickTrigger()
    {
        StartCoroutine(Shoot());
    }

    public override void HandleSwapTrigger()
    {
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        //Dir
        if (isRight) direction = transform.right;
        else direction = -transform.right;
        int i = 0;
        yield return new WaitForSeconds(0.1f);
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


        //  


    }
}
