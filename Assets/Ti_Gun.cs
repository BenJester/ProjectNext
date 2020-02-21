using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ti_Gun : Ti_GunType
{
    // Start is called before the first frame update


    public bool isRight = true;
    Vector2 direction;
    public GameObject bullet;
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

        if (ammoNow > 0)
        {
            yield return new WaitForSeconds(0.2f);
            //Dir
            if (isRight) direction = transform.right;
            else direction = -transform.right;
            //  
            GameObject newBullet = Instantiate(bullet, transform.position + bulletInstanceDistance * (Vector3)direction, Quaternion.identity);
            Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();
            bulletBody.velocity = direction * bulletSpeed;

            ammoNow -= 1;
            SetAmmo();

        }



    }
}
