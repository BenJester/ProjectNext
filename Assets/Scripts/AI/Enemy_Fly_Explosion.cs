using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Fly_Explosion : Enemy
{
    public float speed;
    public float dashPreload;
    public float dashSpeed;
    public float dashDuration;
    public float hitboxWidth;
    public float explosionRadius;
    public LayerMask triggerLayer;
    public float explodePreload;
    public int damage;
    bool dashing;
    bool busy;
    bool triggered;
    public float dashThreshold;
    public float dashCD;
    float currDashCD;
    Rigidbody2D rb;
    BoxCollider2D targetBox;
    BoxCollider2D playerBox;
    public GameObject areaIndicator;
    void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        targetBox = target.GetComponent<BoxCollider2D>();
        playerBox = PlayerControl1.Instance.GetComponent<BoxCollider2D>();
        thing = GetComponent<Thing>();
    }

    // Update is called once per frame
    void Update()
    {
        currDashCD -= Time.deltaTime;
        if (CheckPlayerInSight())
        {
            if (Vector3.Distance(target.transform.position, transform.position) < dashThreshold)
                StartCoroutine(dash());
            else
                StartCoroutine(walk());
        }
    }

    IEnumerator walk()
    {
        if (busy) yield break;
        busy = true;
        Vector2 dir = (target.transform.position - transform.position).normalized;
        rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
        yield return new WaitForEndOfFrame();
        busy = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {

        if (!dashing || triggered) yield break;
        triggered = true;
        thing.sr.color = Color.red;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(explodePreload);
        GameObject area = Instantiate(areaIndicator, transform.position, Quaternion.identity);
        area.transform.parent = null;
        area.GetComponent<SpriteRenderer>().size = new Vector2(explosionRadius * 2, explosionRadius * 2);
        Destroy(area, 0.1f);
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var col in cols)
        {
            if (col.CompareTag("thing"))
            {
                if (col.GetComponent<Thing>().type != Ben.Type.box && col.GetComponent<Thing>().type != Ben.Type.invincible)
                {
                    if (col.GetComponent<Enemy>() != null)
                    {
                        col.GetComponent<Enemy>().TakeDamage(damage);
                    }
                }
                col.GetComponent<Thing>().TriggerMethod?.Invoke();
            }
            else if (col.CompareTag("player"))
            {
                PlayerControl1.Instance.Die();
            }
            if (col.CompareTag("thing") && col.GetComponent<Thing>().TriggerMethod != null)
                col.GetComponent<Thing>().TriggerMethod.Invoke();
        }
        if (!thing.dead)
            thing.Die();
    }

    IEnumerator dash()
    {
        if (busy) yield break;
        if (currDashCD > 0) yield break;
        busy = true;
        exclamation.SetActive(true);
        
        yield return new WaitForSeconds(dashPreload);
        exclamation.SetActive(false);
        Vector2 dir = (target.transform.position - transform.position).normalized;
        float timer = 0f;
        dashing = true;
        rb.velocity = dir * dashSpeed;
        while (timer < dashDuration)
        {
            if (rb.velocity == Vector2.zero && !triggered)
                rb.velocity = dir * dashSpeed;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();

        }
        currDashCD = dashCD;
        rb.velocity = Vector2.zero;
        busy = false;
        dashing = false;
    }
}
