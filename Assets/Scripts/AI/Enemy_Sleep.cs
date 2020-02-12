using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sleep : MonoBehaviour
{
    public float explodeDelay;
    public float sleepDelay;
    public float explodeRadius;
    Vector3 prevPos;
    public float wakeUpThreshold;
    bool wokeUp;
    Animator animator;
    void Start()
    {
        prevPos = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (prevPos != transform.position && (prevPos - transform.position).magnitude < wakeUpThreshold && !wokeUp)
        {
            wokeUp = true;
            StartCoroutine(Explode());
        }
        else if (wokeUp && prevPos == transform.position)
        {
            StartCoroutine(Sleep());
        }
        prevPos = transform.position;
    }

    IEnumerator Sleep()
    {
        yield return new WaitForSeconds(sleepDelay);
        wokeUp = false;
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(1.5f);
        animator.Play("Attack");
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
    }
}
