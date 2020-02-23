using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ti_Rocket : TriggerItem_Base
{
    // Start is called before the first frame update


    public bool isTrigger = false;
    public float speed;
    public float accSpeed;
    public float explosionRadius;

    public GameObject triggerParticle;
    public GameObject explosionAreaIndicator;
    Vector2 kickDir;
    Rigidbody2D my_rb;
    public int damage;
    public float triggerDelay;
    private float setSpeedTime = 0.1f;
    float setTimeTemp;

    void Start()
    {
        my_rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isTrigger)
        {
            my_rb.constraints=RigidbodyConstraints2D.FreezeRotation;
            my_rb.velocity = transform.up*speed;

            speed+=accSpeed;
            if (setTimeTemp<Time.time)
            {
                Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 50f);
                foreach (var col in cols)
                {
                    if (col.CompareTag("floor") || (col.gameObject.layer == 10 && col != GetComponent<Collider2D>()))
                    {
                        StartCoroutine(Explode());
                    }

                    

                }

            }



        }
        else
        {

        }
    }


    public override void HandleKickTrigger()
    {
        kickDir = my_rb.velocity.normalized;
        isTrigger = true;
        my_rb.velocity /= 3;
        //防止误伤
        setTimeTemp = Time.time + setSpeedTime;

    }
    public override void HandleSwapTrigger()
    {
        
        isTrigger = true;
        GameObject temp =Instantiate(triggerParticle,transform.position,transform.rotation);
        temp.transform.localScale*=0.7f;
        Destroy(temp,1f);
        
    }

    public void Trigger()
    {
        StartCoroutine(DoTrigger());
    }

    IEnumerator DoTrigger()
    {
        yield return new WaitForSeconds(triggerDelay);
        HandleSwapTrigger();
    }


    IEnumerator Explode()
    {
        isTrigger = false;
        my_rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.1f);
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        GameObject area = Instantiate(explosionAreaIndicator, transform.position, Quaternion.identity);
        area.transform.parent = null;
        area.GetComponent<SpriteRenderer>().size = new Vector2(explosionRadius * 2, explosionRadius * 2);
        Destroy(area, 0.1f);
        foreach (var col in cols)
        {
            if (col.CompareTag("player"))
            {
                col.GetComponent<PlayerControl1>().Die();
            }
            if (col.CompareTag("thing") && col.GetComponent<Enemy>())
            {
                col.GetComponent<Enemy>().TakeDamage(damage);
            }
            if (col.CompareTag("thing"))
                col.GetComponent<Thing>().TriggerMethod?.Invoke();
        }
        GetComponent<Thing>().Die();
        Destroy(gameObject);
    }

}
