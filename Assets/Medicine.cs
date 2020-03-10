using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Medicine : MonoBehaviour
{
    // Start is called before the first frame update
    bool isChecking = false;
    Rigidbody2D rb;
    LineRenderer lr;
    public float radius;
    public GameObject indicator;
    float startTime;
    public float medineTime;
    public GameObject bullet;
    public GameObject prepareParticle;
    public float bulletInstanceDistance;

    GameObject enemyTarget;
    public bool StartMec = false;
    public PlayerControl1 pc;
    public float bulletSpeed;
    Vector2 direction;
    void Start()
    {
        pc = PlayerControl1.Instance;
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        //GameObject area = Instantiate(indicator, transform.position, Quaternion.identity);
        //area.transform.parent = null;
        //area.GetComponent<SpriteRenderer>().size = new Vector2(radius * 2, radius * 2);
        //area.GetComponent<SpriteRenderer>().color = Color.green;
        //area.transform.parent = transform;

    }

    
    void Update()
    {

        if (!isChecking) {
            lr.startColor = Color.yellow;
            lr.endColor = Color.yellow;
            lr.startWidth = 5;
            lr.enabled = false;

            Check();
            
        } 
        else {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, enemyTarget.transform.position);
        }

        if (CheckPlayer())
        {
            //DOTween.Kill(gameObject);
            if (!StartMec)
            {
                StartMec = true;
                startTime = Time.time;
                //lr.enabled = true;
            }

            if (Time.time - startTime >= medineTime )
            {
                //lr.SetPosition(0, transform.position);
                //lr.SetPosition(1, pc.transform.position);
                //StartMedicine();
            }

        }
        else {

            //MoveToPlay();
            StartMec = false;
            
        }
    }

    bool CheckPlayer() {
        return Vector2.Distance(pc.transform.position, transform.position) <= radius; 
    
    }

    void  StartMedicine() {
       
            pc.hp += 1;
            pc.hp = Mathf.Clamp(pc.hp, 0, pc.maxhp);  
    }
    //void MoveToPlay() {
    //    if (!StartMec) rb.velocity = (-(transform.position - pc.transform.position).normalized)*100;
    //}

    void Check() {
        Collider2D[] list = Physics2D.OverlapCircleAll(transform.position, radius, 1<<10);
        foreach (var item in list)
        {
            if (item.gameObject != gameObject) {
                enemyTarget = item.gameObject;
                StartCoroutine(BeginShoot(item.gameObject));
                isChecking = true;
                lr.enabled = true;
                return;
            }
        }
        
    }

    void Shoot(GameObject target) {
        direction = (target.transform.position - transform.position).normalized;
        lr.startColor = Color.red;
        lr.endColor = Color.red;
        lr.startWidth = 25;
       
        target.GetComponent<Enemy>().TakeDamage(2);
        
        //GameObject newBullet = Instantiate(bullet, transform.position + bulletInstanceDistance * (Vector3)direction, Quaternion.identity);
        //Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();
        //bulletBody.velocity = direction * bulletSpeed;

        isChecking = false;

    }

    IEnumerator BeginShoot(GameObject target) {
        GameObject part = Instantiate(prepareParticle, transform.position,Quaternion.identity);
        Destroy(part, 1.5f);
        yield return new WaitForSeconds(1.5f);
        Shoot(target);

    
    
    }
}
