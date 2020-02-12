using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ti_Gun : TriggerItem_Base
{
    // Start is called before the first frame update
    

    public bool isRight=true;
     Vector2 direction;
    public GameObject bullet;
    public float bulletSpeed;
    public float bulletInstanceDistance = 50f;

    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void HandleTriggerAction(){
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot(){
        yield return new WaitForSeconds(0.2f);
        //Dir
        if(isRight) direction = transform.right;
        else direction = -transform.right;
        //  

        GameObject newBullet = Instantiate (bullet, transform.position + bulletInstanceDistance * (Vector3) direction, Quaternion.identity);
		Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D> ();
		bulletBody.velocity = direction * bulletSpeed;
    }
}
