using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Fly : Enemy
{
    public float speed;
    public float dashPreload;
    public float dashSpeed;
    public float dashDuration;
    public float hitboxWidth;
    bool busy;
    public float dashThreshold;
    public float dashCD;
    float currDashCD;
    Rigidbody2D rb;
    BoxCollider2D targetBox;
    BoxCollider2D playerBox;
    void Start()
    {
        base.Start();
        rb  = GetComponent<Rigidbody2D>();
        targetBox = target.GetComponent<BoxCollider2D>();
        playerBox = PlayerControl1.Instance.GetComponent<BoxCollider2D>();
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

    IEnumerator dash()
    {
        if (busy) yield break;
        if (currDashCD > 0) yield break;
        busy = true;
        exclamation.SetActive(true);
        Vector2 dir = (target.transform.position - transform.position).normalized;
        yield return new WaitForSeconds(dashPreload);
        exclamation.SetActive(false);
        
        float timer = 0f;
        while (timer < dashDuration)
        {
            Vector2 vecSource = new Vector2(transform.position.x, transform.position.y);
            Physics2D.IgnoreCollision(box, targetBox, true);
            Physics2D.IgnoreCollision(box, playerBox, true);
            rb.MovePosition(rb.position + dir * dashSpeed * Time.deltaTime);
            float fAngle = Vector2.SignedAngle(transform.position, target.transform.position);
            Collider2D[] cols = Physics2D.OverlapBoxAll(vecSource + dir * (box.size.x + hitboxWidth),
                             new Vector2(hitboxWidth * 2, box.size.y),
                             fAngle);

            foreach (var col in cols)
            {
                if (col.CompareTag("thing") && col.gameObject != gameObject)
                {
                    if (col.GetComponent<Thing>().type != Ben.Type.box)
                        col.GetComponent<Thing>().Die();
                }
                else if (col.CompareTag("player"))
                {

                    PlayerControl1.Instance.Die();
                }
            }
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
            Physics2D.IgnoreCollision(box, targetBox, false);
            Physics2D.IgnoreCollision(box, playerBox, false);
        }
        currDashCD = dashCD;
        rb.velocity = Vector2.zero;
        busy = false;
    }
}
