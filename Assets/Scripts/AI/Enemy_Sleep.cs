using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sleep : Enemy
{
    public float explodeDelay;
    public float sleepDelay;
    public float explodeRadius;
    Vector3 prevPos;
    public float wakeUpThreshold;
    bool wokeUp;
    Animator animator;

    public GameObject areaIndicator;

    void Start()
    {
        base.Start();
        prevPos = transform.position;
        animator = GetComponent<Animator>();
        animator.Play("Stop");
    }

    public void WakeUp()
    {
        if(wokeUp) return;
        StartCoroutine(Explode());
    }

    void Update()
    {
        //if (prevPos != transform.position && (prevPos - transform.position).magnitude < wakeUpThreshold && !wokeUp)
        //{

        //    StartCoroutine(Explode());
        //}
        //else if (wokeUp && prevPos == transform.position)
        //{
        //    StartCoroutine(Sleep());
        //}
        //prevPos = transform.position;
    }

    IEnumerator Sleep()
    {
        yield return new WaitForSeconds(sleepDelay);
        wokeUp = false;
        animator.Play("Stop");
    }

    IEnumerator Explode()
    {
        GameObject area = Instantiate(areaIndicator, transform.position, Quaternion.identity);
        area.transform.parent = null;
        area.GetComponent<SpriteRenderer>().size = new Vector2(explodeRadius * 2, explodeRadius * 2);
        area.transform.parent = transform;

        animator.Play("Idle");
        wokeUp = true;
        yield return new WaitForSeconds(1.5f);
        animator.Play("Attack");
        area.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(explodeDelay - 1.5f);
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explodeRadius);
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
        Destroy(area);
        StartCoroutine(Sleep());
    }
}
