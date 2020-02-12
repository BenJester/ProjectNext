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

    void Start()
    {
        my_rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTrigger)
        {
            transform.localRotation = Quaternion.Euler(0, 0, AngleBetween(Vector2.up, kickDir.normalized));
            my_rb.velocity = kickDir * speed;
        }
        else
        {
            my_rb.velocity=Vector2.zero;
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
        my_rb.freezeRotation = false;
        isTrigger = true;
        kickDir = my_rb.velocity.normalized;

    }

    public override void HandleSwapTrigger()
    {

    }


    void Fly()
    {

    }

    


    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isTrigger)
        {
            if (other.transform.CompareTag("floor"))
            {
                my_rb.velocity = Vector2.zero;
                my_rb.freezeRotation = true;
                Debug.Log("Stop");
                isTrigger = false;
            }
            if (other.transform.CompareTag("player"))
            {
                other.transform.GetComponent<PlayerControl1>().Die();
            }
            if (other.transform.CompareTag("thing") && other.transform.GetComponent<Enemy>())
            {
                other.transform.GetComponent<Enemy>().TakeDamage(1);
            }
        }

    }



}
