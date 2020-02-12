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
    public float kickFloorInputRadius;
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
        Vector2 dir = (kickPos - transform.position).normalized;
        float angle = Angle(dir);
        //if (0f < angle &&
    }

    public static float Angle(Vector2 p_vector2)
    {
        if (p_vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
        }
    }

    //Swap 触发Trigger Instance
    void TriggerInstanceEvent(Thing swapThing){
        if(swapThing.GetComponent<TriggerItem_Base>()!=null){
            TriggerItem_Base tb = swapThing.GetComponent<TriggerItem_Base>();
            tb.HandleTriggerAction();
        }
    }

    IEnumerator DoPull(Rigidbody2D rb)
    {
        if (swap.overheadSnap && rb == swap.overheadRB)
        {
            swap.DropOverhead();
        }
        Vector3 targetPos = playerControl.transform.position + new Vector3(0f, pullHeight, 0f);
        Vector3 diff = targetPos - rb.transform.position;
        //rb.GetComponent<BoxCollider2D>().enabled = false;
        int layer = rb.gameObject.layer;
        rb.gameObject.layer = 18;
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
        rb.gameObject.layer = layer;


        //触发
        TriggerInstanceEvent(rb.GetComponent<Thing>());

        yield return new WaitForSeconds(0.2f);
        //
        //target.GetComponent<BoxCollider2D>().enabled = true;
    }
}
