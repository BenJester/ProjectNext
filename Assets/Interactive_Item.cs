using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Interactive_Item : MonoBehaviour
{
    // Start is called before the first frame update


    Rigidbody2D rb;
    public bool isRotate=false;
    public float speed;
    public float rotateSpeed;
    public float speedAcc;
    public float maxSpeed;


    public Transform target;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = PlayerControl1.Instance.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotate)
        {
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rb.angularVelocity = -rotateAmount * rotateSpeed;
            //if (isAddVelocity)
            //{
                rb.velocity += (Vector2)transform.up * speedAcc;
                rb.velocity = Mathf.Clamp(rb.velocity.magnitude, 0f, maxSpeed) * rb.velocity.normalized;
            //}
            //else
            //{
            //    rb.velocity = (Vector2)transform.up * startSpeed;
            //    startSpeed = Mathf.Clamp(0f, startSpeed + speedAcc, maxSpeed);
            //}


        }
    }

    public void FlyToPlayer() {
        StopAllCoroutines();
        StartCoroutine(Fly());
        
    }


    IEnumerator Fly() {
        Vector2 dir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle-90, new Vector3(0,0,1));
        yield return new WaitForSeconds(0.3f);
        transform.DOMove(target.position, 0.5f, true).SetEase(Ease.InSine);
    }

    public void SetFlyKnifeTrigger() {
        isRotate = true;
        GetComponent<SpriteRenderer>().color = Color.red;



    }

    public void Disapear() { 
        
    
    }


    
}
