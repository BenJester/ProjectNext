using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ti_Gun : Ti_GunType,TriggerItem_Base
{
    // Start is called before the first frame update

    public float timePreFire;
    public bool isRight = true;
    Vector2 direction;
    public GameObject bullet;
    public float bulletSpeed;
    public float bulletInstanceDistance = 50f;
    public float triggerDelay;
    Color originalColor;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalColor = GetComponent<SpriteRenderer>().color;
        if (!isRight)
        {
            GetComponent<SpriteRenderer>().flipX = true;

        } else {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        //SetDirection();
    }

    public void HandleKickTrigger()
    {
        StartCoroutine(Shoot());
    }

    public void HandleSwapTrigger()
    {

        transform.SetParent(null);
        rb.bodyType=RigidbodyType2D.Dynamic;
        StartCoroutine(Shoot());
    }

    public void Trigger()
    {
        StartCoroutine(DoTrigger());
    }

    IEnumerator DoTrigger()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        
        sr.color = Color.red;

        yield return new WaitForSeconds(triggerDelay);
        sr.color = originalColor;
        StartCoroutine(Shoot());
    }

    //这个是修改枪械的代码
    void SetDirection(){
        if(canRotate && GetComponent<Rigidbody2D>().velocity.x!=0){
            if(GetComponent<Rigidbody2D>().velocity.x>0) isRight =true;
            else isRight=false;
        }
        
    }
    public IEnumerator Shoot()
    {

        if (ammoNow > 0)
        {
            yield return new WaitForSeconds(timePreFire);
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
        else {
            Destroy(gameObject);
        }



    }
}
