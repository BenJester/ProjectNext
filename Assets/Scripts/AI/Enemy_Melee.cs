using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Melee : Enemy
{
    public float walkSpeed;
    public float hitboxWidth;
    public float hitboxHeight;
    public float hitboxOffset;
    public float attackDelay;
    public float attackPostDelay;
    public float idleDur;
    public float dashDur;
    public float dashSpeed;
    bool busy;
    Rigidbody2D body;
    public LayerMask checkLayer;
    public LayerMask hitLayer;
    public bool faceRight;
    Animator animator;
    Vector3 prevPos;
    void Start()
    {
        base.Start();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        prevPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CheckPlayerInSight() || thing.dead) return;
        StartCoroutine(Walk());
        prevPos = transform.position;
    }

    bool CheckRange()
    {
        //return false;
        if (faceRight)
            return Physics2D.OverlapArea
                (
                    (Vector2)transform.position + new Vector2(-hitboxWidth / 2f + hitboxOffset, hitboxHeight / 2f),
                    (Vector2)transform.position + new Vector2(hitboxWidth / 2f + hitboxOffset, -hitboxHeight),
                    checkLayer
                );
        else
            return Physics2D.OverlapArea
                (
                    (Vector2)transform.position + new Vector2(-hitboxWidth / 2f - hitboxOffset, hitboxHeight / 2f),
                    (Vector2)transform.position + new Vector2(hitboxWidth / 2f - hitboxOffset, -hitboxHeight),
                    checkLayer
                );
    }
    IEnumerator Idle()
    {
        if (busy) yield break;
        busy = true;
        animator.Play("Idle");
        yield return new WaitForSeconds(idleDur);
        busy = false;
    }
    IEnumerator Walk()
    {
        if (busy) yield break;
        busy = true;
        animator.Play("Walk");

        // && Mathf.Abs(transform.position.x - PlayerControl1.Instance.transform.position.x) > 5f
        while (!CheckRange() && CheckPlayerInSight() && Mathf.Abs(transform.position.x - PlayerControl1.Instance.transform.position.x) > 5f)
        {
            if (body.velocity.y == 0f)
                body.velocity = new Vector2(PlayerControl1.Instance.transform.position.x < transform.position.x ? -walkSpeed : walkSpeed, body.velocity.y);
            faceRight = PlayerControl1.Instance.transform.position.x < transform.position.x ? false : true;
            GetComponent<SpriteRenderer>().flipX = !faceRight;
            //if (!grounded) break;
            yield return new WaitForEndOfFrame();
        }
        busy = false;
        if (CheckRange())
            StartCoroutine(Attack());
        else
            StartCoroutine(Idle());

    }
    IEnumerator Attack()
    {
        if (busy) yield break;
        if (!CheckRange()) yield break;
        busy = true;
        animator.StopPlayback();
        animator.Play("Attack");
        yield return new WaitForSeconds(attackDelay);
        Collider2D[] cols = null;
        float currTime = 0f;
        body.velocity = new Vector2(faceRight ? dashSpeed : -dashSpeed, body.velocity.y);
        while (currTime < dashDur)
        {
            
            if (faceRight)
                cols = Physics2D.OverlapAreaAll
                        (
                            (Vector2)transform.position + new Vector2(-hitboxWidth / 2f + hitboxOffset, hitboxHeight / 2f),
                            (Vector2)transform.position + new Vector2(hitboxWidth / 2f + hitboxOffset, -hitboxHeight / 2f),
                            hitLayer
                        );
            else
                cols = Physics2D.OverlapAreaAll
                        (
                            (Vector2)transform.position + new Vector2(-hitboxWidth / 2f - hitboxOffset, hitboxHeight / 2f),
                            (Vector2)transform.position + new Vector2(hitboxWidth / 2f - hitboxOffset, -hitboxHeight / 2f),
                            hitLayer
                        );
            if (thing.dead) yield break;
            foreach (var col in cols)
            {
                if (col.CompareTag("thing") && col.gameObject != gameObject)
                {
                    col.GetComponent<Thing>().TriggerMethod?.Invoke();

                    if (col.GetComponent<Enemy>() != null)
                        col.GetComponent<Enemy>().TakeDamage(1);


                }
                else if (col.CompareTag("player"))
                {

                    PlayerControl1.Instance.Die();
                }
            }
            currTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        body.velocity = new Vector2(0f, body.velocity.y);
        yield return new WaitForSeconds(attackPostDelay);
        busy = false;
    }
}
