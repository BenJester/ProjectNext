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

    IEnumerator Walk()
    {
        if (busy) yield break;
        busy = true;
        if (prevPos != transform.position)
            animator.Play("Walk");
        else
            animator.Play("Idle");
        // && Mathf.Abs(transform.position.x - PlayerControl1.Instance.transform.position.x) > 5f
        while (!CheckRange() && CheckPlayerInSight() && Mathf.Abs(transform.position.x - PlayerControl1.Instance.transform.position.x) > 5f)
        {
            body.velocity = new Vector2(PlayerControl1.Instance.transform.position.x < transform.position.x ? -walkSpeed : walkSpeed, body.velocity.y);
            faceRight = PlayerControl1.Instance.transform.position.x < transform.position.x ? false : true;
            GetComponent<SpriteRenderer>().flipX = !faceRight;
            //if (!grounded) break;
            yield return new WaitForEndOfFrame();
        }
        busy = false;
        if (CheckRange())
            StartCoroutine(Attack());
        
        
    }
    IEnumerator Attack()
    {
        if (busy) yield break;
        busy = true;
        animator.StopPlayback();
        animator.Play("Attack");
        yield return new WaitForSeconds(attackDelay);
        Collider2D[] cols = null;
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
        yield return new WaitForSeconds(attackPostDelay);
        busy = false;
    }
}
