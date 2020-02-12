using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ti_Rocket : TriggerItem_Base
{
    // Start is called before the first frame update


    public bool isTrigger = false;
    public float speed;
    public float explosionRadius;
    public GameObject explosionAreaIndicator;
    Vector2 kickDir;
    Rigidbody2D my_rb;

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
            my_rb.constraints = RigidbodyConstraints2D.None;
            my_rb.velocity += kickDir * speed;
            transform.localRotation = Quaternion.Euler(0, 0, Dash.AngleBetween(Vector2.up, kickDir.normalized));
        }
    }


    public override void HandleKickTrigger()
    {
        kickDir = my_rb.velocity.normalized;
        isTrigger = true;
        my_rb.velocity/=10;
        //防止误伤
        setTimeTemp = Time.time + setSpeedTime;

    }
    public override void HandleSwapTrigger()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isTrigger)
        {

            if (other.transform.CompareTag("player") && setTimeTemp < Time.time)
            {
                StartCoroutine(Explode());
            }
            if (other.transform.CompareTag("thing") && other.transform.GetComponent<Enemy>())
            {
                StartCoroutine(Explode());
            }
            if (other.transform.CompareTag("floor"))
            {
                StartCoroutine(Explode());
            }
        }
    }


   

    IEnumerator Explode()
    {
        isTrigger=false;
        my_rb.velocity=Vector2.zero;
        yield return new WaitForSeconds(0.2f);
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
                col.GetComponent<Enemy>().TakeDamage(1);
            }
        }
        GetComponent<Thing>().Die();
        Destroy(gameObject);
    }

    }
