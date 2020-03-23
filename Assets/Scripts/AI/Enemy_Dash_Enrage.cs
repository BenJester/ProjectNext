using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Dash_Enrage : Enemy
{
    LineRenderer lr;
    Transform player;
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        player = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();
        base.Start();
    }
    public void TempShield()
    {
        StartCoroutine(DoTempShield());
    }

    IEnumerator DoTempShield()
    {
        thing.SetShield(true);
        yield return new WaitForSeconds(dashDur);
        thing.SetShield(false);
    }
    void Update()
    {
        Chase();
    }
    public float speed;

    void Chase()
    {
        if (thing.dead) return;
        if (CheckPlayerInSight() && !thing.beingThrown && !busy)
            rb.velocity = (player.position - transform.position).normalized * speed;
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 50f);

        foreach (var col in cols)
        {
            if (col.CompareTag("player"))
            {
                col.GetComponent<PlayerControl1>().Die();
            }


        }
    }

    public float dashSpeed;
    public float dashDur;
    public Vector2 dir;
    public float dashPreload;
    public float hitboxWidth;
    public int damage;
    bool busy;

    public void DoDash()
    {
        StartCoroutine(Dash());
    }
    IEnumerator Dash()
    {
        if (busy) yield break;
        busy = true;
        float timer = 0f;
        exclamation.SetActive(true);
        while(timer < dashPreload || thing.beingThrown)
        {
            dir = (player.transform.position - transform.position).normalized;
            lr.enabled = true;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position + (Vector3)dir * 2000f);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        timer = 0f;
        exclamation.SetActive(false);
        dir = (player.transform.position - transform.position).normalized;
        rb.velocity = dir * dashSpeed;
        gameObject.layer = 18;
        float drag = rb.drag;
        rb.drag = 0f;
        Color color = m_spRender.color;
        m_spRender.color = Color.red;
        while (timer < dashDur)
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position + (Vector3)rb.velocity.normalized * 2000f);
            timer += Time.fixedDeltaTime;
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 50f);

            foreach (var col in cols)
            {
                if (col.CompareTag("player"))
                {
                    col.GetComponent<PlayerControl1>().Die();
                }
                if (col.CompareTag("thing") && col.GetComponent<Enemy>() && col.gameObject != gameObject)
                {
                    col.GetComponent<Enemy>().TakeDamage(damage);
                }
                if (col.CompareTag("thing"))
                    col.GetComponent<Thing>().TriggerMethod?.Invoke();
                
            }
            yield return new WaitForFixedUpdate();
        }
        m_spRender.color = color;
        lr.enabled = false;
        rb.drag = drag;
        gameObject.layer = 10;
        rb.velocity = Vector2.zero;
        busy = false;
    }
}
