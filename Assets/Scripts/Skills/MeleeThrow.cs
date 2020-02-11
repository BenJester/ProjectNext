using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeThrow : Skill
{

    public float range;
    public float throwSpeed;
    Collider2D col;
    Swap swap;
    bool pulled;
    public float pullSpeed;
    public float pullHeight;
    public float snapThreshold;
    public Rigidbody2D target;
    public GameObject dashPointer;
    private void Start()
    {
        swap = GetComponent<Swap>();
    }

    public override bool Check()
    {
        if (playerControl.closestObjectToPlayer && playerControl.closestPlayerDistance <= range)
        {
            col = playerControl.closestObjectToPlayer.GetComponent<Collider2D>();
            return true;
        }
        else
            return false;
    }

    void Update()
    {
       if(Input.GetMouseButton(1)){
            Time.timeScale = 0.1f;
            Time.fixedDeltaTime *= 0.1f;
       }
            
        if(Input.GetMouseButtonUp(1))
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime *= 10f;
            Do();
        }
        Check();
        Vector2 dir = Vector2.zero;
        if (col != null)
            dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - col.transform.position).normalized;
        if (Input.GetMouseButton(1) && col != null)
        {
            dashPointer.SetActive(true);

            dashPointer.transform.position = (Vector2)col.transform.position + dir * 70f;
            dashPointer.transform.localRotation = Quaternion.Euler(0, 0, -Dash.AngleBetween(Vector2.up, dir));

        }
        else if (Input.GetMouseButtonUp(1))
        {
            dashPointer.SetActive(false);
        }
    }

    public override void Do()
    {
        if (!active || !Check())
            return;
        if (col != null)
        {
            StartCoroutine(DoPull(col.GetComponent<Rigidbody2D>()));
        }

    }

    void KickFloor(Vector3 kickPos)
    {
        

    }

    IEnumerator DoPull(Rigidbody2D rb)
    {
        if (swap.overheadSnap && rb == swap.overheadRB)
        {
            swap.DropOverhead();
        }
        Vector3 targetPos = playerControl.transform.position + new Vector3(0f, pullHeight, 0f);
        Vector3 diff = targetPos - rb.transform.position;
        rb.GetComponent<BoxCollider2D>().enabled = false;

        while (diff.magnitude > snapThreshold)
        {
            Debug.Log(diff.magnitude);
            targetPos = playerControl.transform.position + new Vector3(0f, pullHeight, 0f);
            diff = targetPos - rb.transform.position;
            rb.velocity = diff.normalized * pullSpeed;
            yield return new WaitForEndOfFrame();
        }
        rb.transform.position = playerControl.transform.position + new Vector3(0f, pullHeight, 0f);
        rb.bodyType = RigidbodyType2D.Kinematic;
        target = rb;
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mouseWorldPos - (Vector2)target.transform.position).normalized;
        target.gameObject.GetComponent<Rigidbody2D>().velocity = dir * throwSpeed;
        pulled = false;
        target.transform.parent = null;
        target.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(0.2f);
        target.GetComponent<BoxCollider2D>().enabled = true;
    }
}
