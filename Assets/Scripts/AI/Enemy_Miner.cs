using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Miner : Enemy
{
    public float runDur;
    public float speed;
    bool busy;
    public float explosionDelay;
    public float explosionRadius;
    public GameObject explosionAreaIndicator;
    public int damage;
    bool triggered;
    private void Start()
    {
        base.Start();
        StartCoroutine (HandleShoot ());
        m_spRender = GetComponent<SpriteRenderer>();
    }

    public void SelfDestroy()
    {
        StartCoroutine(DoRun());
    }

    IEnumerator DoRun()
    {
        if (triggered) yield break;
        triggered = true;
        
        float timer = 0f;
        while (timer < runDur)
        {
            timer += 0.2f;
            if (player.transform.position.x > transform.position.x)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
            yield return new WaitForSeconds(0.2f);
        }
        StartCoroutine(Explode());
    }
    public IEnumerator Explode()
    {
        if (busy) yield break;
        busy = true;
        GetComponent<SpriteRenderer>().color = Color.black;
        rb.velocity = new Vector2(0f, rb.velocity.y);
        yield return new WaitForSeconds(explosionDelay);


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
        thing.Die();
    }

    [Space]
    [Header("关闭地话平常就不会发子弹")]
    public bool canShoot = true;

    public bool aimNoCollider = false;
    public float distance;
    public GameObject bullet;
    private Vector2 direction;
    public float bulletSpeed;
    public float shootInterval = 1f;
    public float animationPreload = 0.1f;
    public float bulletInstanceDistance = 50f;
    private Animator animator;
    public Transform player;
    public bool isInSight = false;
    public LineRenderer lr;


    protected void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();
        lr = GetComponent<LineRenderer>();
    }


    // Update is called once per frame
    void Update()
    {

        if (thing.dead)
        {
            lr.enabled = false;
            return;
        }
        direction = (player.position - transform.position).normalized;
        //在视线中
        isInSight = CheckPlayerInSight();

        //画线；
        if (isInSight)
        {
            lr.enabled = true;
            Debug.DrawLine(transform.position, transform.position + (Vector3)direction * distance, Color.red, 0.1f);
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, (player.position - transform.position).normalized * distance + transform.position);
        }
        else lr.enabled = false;

    }

    protected IEnumerator HandleShoot()
    {

        while (canShoot && !triggered)
        {
            if (isInSight)
            {

                animator.CrossFade("Enemy_Shooter_Shot", 0.001f);
                exclamation.SetActive(true);
                yield return new WaitForSeconds(animationPreload);
                Shoot();
                exclamation.SetActive(false);
                yield return new WaitForSecondsRealtime((shootInterval - animationPreload) / Time.timeScale);
            }
            else
            {
                yield return null;
            }
        }

    }

    public void Shoot()
    {
        if (thing.dead)
            return;
        GameObject newBullet = Instantiate(bullet, transform.position + bulletInstanceDistance * (Vector3)direction, Quaternion.identity);
        Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();
        bulletBody.velocity = direction * bulletSpeed;

    }

    public void PowerShoot()
    {
        if (thing.dead)
            return;
        GameObject newBullet = Instantiate(bullet, transform.position + bulletInstanceDistance * (Vector3)direction, Quaternion.identity);
        Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();
        bulletBody.velocity = direction * bulletSpeed;
        newBullet.GetComponent<EnemyBullet>().damage = 2;

        newBullet.transform.localScale *= 1.5f;


        //GetComponent<Thing>().SetShield(true);
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

    public bool CheckPlayerInSight()
    {


        if (aimNoCollider)
        {
            if (Vector2.Distance(transform.position, player.position) <= distance) return true;
            else return false;
        }

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, (player.position - transform.position).normalized, distance, (1 << 10) | (1 << 8) | (1 << 9));
        RaycastHit2D hitNear;
        if (hits.Length >= 2)
        {
            hitNear = hits[1];
            if (hitNear.collider.tag == "player") return true;
            else return false;
        }
        else return false;
    }
    private void OnDrawGizmos()
    {
        if (player != null) Gizmos.DrawLine(transform.position, (player.position - transform.position).normalized * distance + transform.position);
    }

}
