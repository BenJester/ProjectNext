using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ti_FlyKnife : TriggerItem_Base
{
    // Start is called before the first frame update
    public bool isTrigger = false;
    public float speed;
    Vector2 kickDir;
    Rigidbody2D my_rb;
    private float setSpeedTime = 0.1f;
    float setTimeTemp;

    void Start()
    {
        my_rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {


    }


    void FixedUpdate()
    {
        if (isTrigger)
        {
            //my_rb.constraints = RigidbodyConstraints2D.None;
            
            transform.localRotation = Quaternion.Euler(0, 0, AngleBetween(Vector2.up, kickDir.normalized));


            //检测墙壁停止
            //RaycastHit2D hit2D = Physics2D.Raycast(transform.position, kickDir, 20, 1 << 8);
            //if (hit2D)
            //{
            //    transform.position = (Vector2)transform.position + (Vector2)kickDir * 15;
            //    my_rb.velocity = Vector2.zero;
            //    my_rb.freezeRotation = true;
            //    print("Hit");
            //    //my_rb.bodyType=RigidbodyType2D.Static;
            //    isTrigger = false;
            //}
        }
    }

    public static float AngleBetween(Vector2 vectorA, Vector2 vectorB)
    {
        float angle = Vector2.Angle(vectorA, vectorB);
        Vector3 cross = Vector3.Cross(vectorA, vectorB);

        if (cross.z > 0)
        {
            angle = 360 - angle;
        }

        return angle;
    }

    public override void HandleKickTrigger()
    {
        //my_rb.bodyType = RigidbodyType2D.Kinematic;
        //my_rb.freezeRotation = false;
        //isTrigger = true;
        kickDir = my_rb.velocity.normalized;
        //print("Kick!!");

        //防止误伤
        setTimeTemp = Time.time + setSpeedTime;
        my_rb.velocity = kickDir * speed;
    }


    public override void HandleSwapTrigger()
    {
    }


    void Fly()
    {

    }


    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("floor") || (col.collider.gameObject.layer == 12))
        {
            my_rb.velocity = Vector2.zero;
        }
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        //if (isTrigger)
        //{

        //    if (other.transform.CompareTag("player") && setTimeTemp<Time.time)
        //    {
        //        other.transform.GetComponent<PlayerControl1>().Die();
        //    }
        //    if (other.transform.CompareTag("thing") && other.transform.GetComponent<Enemy>())
        //    {
        //        other.transform.GetComponent<Enemy>().TakeDamage(1);
        //    }
        //}
        //else if (other.transform.CompareTag("floor"))
        //{
        //    my_rb.velocity = Vector2.zero;
        //    my_rb.freezeRotation = true;
        //    //my_rb.constraints=RigidbodyConstraints2D.FreezePosition;

        //}

    }

    private void OnTriggerStay2D(Collider2D other) {
        //if (other.transform.CompareTag("floor"))
        //{
        //    my_rb.velocity = Vector2.zero;
        //    my_rb.freezeRotation = true;
        //    //my_rb.constraints=RigidbodyConstraints2D.FreezePosition;

        //}
    }




}
