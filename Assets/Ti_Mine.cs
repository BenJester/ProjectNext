using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ti_Mine : TriggerItem_Base
{
    //TODO: 未来可能可以做成会黏在墙壁上的
    public bool isTrigger = false;

    public float explosionRadius;
    public float checkExpRaius;

    public float timeAfterCheck = 0.2f;
    public int damage = 1;
    private float tempTimer;

    public GameObject explosionAreaIndicator;
    public GameObject checkAreaIndicator;


    GameObject checkAreaTemp;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isTrigger)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, checkExpRaius);
            foreach (var col in cols)
            {
                if ((col.CompareTag("player") || (col.CompareTag("thing")))&& col!=GetComponent<Collider2D>())
                {
                    StartCoroutine(StartExplosion());
                }
                
            }
        }
    }

    public override void HandleSwapTrigger()
    {
        if (!isTrigger) StartCoroutine(TriggerBomb());

    }

    public override void HandleKickTrigger()
    {
        if (!isTrigger) StartCoroutine(TriggerBomb());

    }


    IEnumerator TriggerBomb()
    {
       
        //交换和投掷留出的触发时间
        yield return new WaitForSeconds(0.5f);
        isTrigger = true;
         GetComponent<SpriteRenderer>().color = Color.red;
        checkAreaTemp = Instantiate(checkAreaIndicator, transform.position, Quaternion.identity);
        checkAreaTemp.transform.parent = null;
        checkAreaTemp.GetComponent<SpriteRenderer>().size = new Vector2(checkExpRaius * 2, checkExpRaius * 2);
    }




    IEnumerator StartExplosion()
    {
        Destroy(checkAreaTemp);
        yield return new WaitForSeconds(timeAfterCheck);
        Explode();
    }

    void Explode()
    {
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
        }
        GetComponent<Thing>().Die();
        Destroy(gameObject);
    }
}
