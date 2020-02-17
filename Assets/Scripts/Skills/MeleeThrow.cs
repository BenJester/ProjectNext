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
    public float kickFloorSpeed;
    Vector2 kickFloorDir;
    LineRenderer lr;
    private void Start()
    {
        swap = GetComponent<Swap>();
        lr = GetComponent<LineRenderer>();
    }

    public override bool Check()
    {
        if (playerControl.closestObjectToPlayer && playerControl.closestPlayerDistance <= range)
        {
            col = playerControl.closestObjectToPlayer.GetComponent<Collider2D>();
            return true;
        }
        else
        {
            col = null;
            return false;
        }
           
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Time.timeScale = 0.1f;
            Time.fixedDeltaTime *= 0.1f;
        }

        if (Input.GetMouseButtonUp(1))
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
            lr.enabled = true;
            lr.SetPositions(PlotTrajectory(col.transform.position, dir * throwSpeed, 0.05f, 24));

        }
        else if (Input.GetMouseButton(1) && CanKickFloor(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            dashPointer.SetActive(true);

            dashPointer.transform.position = (Vector2)transform.position - kickFloorDir * 70f;
            dashPointer.transform.localRotation = Quaternion.Euler(0, 0, -Dash.AngleBetween(Vector2.up, -kickFloorDir));
        }
        else if (Input.GetMouseButtonUp(1))
        {
            dashPointer.SetActive(false);
            lr.enabled = false;
        }
        //Debug.Log(Angle(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position));
    }

    public override void Do()
    {
        if (!active)
            return;
        if (col != null && Check())
        {
            StartCoroutine(DoPull(col.GetComponent<Rigidbody2D>()));
        }
        else
        {
            KickFloor(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

    }

    //Swap 触发Trigger Instance
    void TriggerInstanceEvent(Thing swapThing)
    {
        if (swapThing.GetComponent<TriggerItem_Base>() != null)
        {
            TriggerItem_Base tb = swapThing.GetComponent<TriggerItem_Base>();
            tb.HandleKickTrigger();
        }
    }

    void KickFloor(Vector3 kickPos)
    {
        
        if (Input.GetMouseButtonUp(1))
        {
            if (CanKickFloor(kickPos)) StartCoroutine(DoKickFloor(kickFloorDir));
        }
        
        //Debug.Log(dir.x);
    }

    bool CanKickFloor(Vector3 kickPos)
    {
        return false;
        if ((kickPos - transform.position).magnitude > kickFloorInputRadius || (kickPos - transform.position).magnitude < box.size.x / 2f)
            return false;

        kickFloorDir = (kickPos - transform.position).normalized;
        Vector3 pos = transform.position;
        float diffX = kickPos.x - pos.x;
        float diffY = kickPos.y - pos.y;
        float boxWidth = box.size.x / 2f;
        float boxHeight = box.size.y / 2f;

        return (diffX < -boxWidth && playerControl.touchingWallLeft()) || (diffX > boxWidth && playerControl.touchingWallRight())
            || (diffY < -boxHeight && playerControl.touchingFloor()) || (diffY > boxHeight && playerControl.touchingWallUp());
    }

    IEnumerator DoKickFloor(Vector2 dir)
    {
        playerBody.velocity = -dir * kickFloorSpeed;
        playerControl.disableAirControl = true;
        yield return new WaitForSeconds(0.15f);
        playerControl.disableAirControl = false;
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

    IEnumerator DoPull(Rigidbody2D rb)
    {
        if (rb.GetComponent<Thing>().hasShield) yield break;
        if (swap.overheadSnap && rb == swap.overheadRB)
        {
            swap.DropOverhead();
        }
        Vector3 targetPos = playerControl.transform.position + new Vector3(0f, pullHeight, 0f);
        Vector3 diff = targetPos - rb.transform.position;
        //rb.GetComponent<BoxCollider2D>().enabled = false;
        int layer = rb.gameObject.layer;
        rb.gameObject.layer = 19;
        //while (diff.magnitude > snapThreshold)
        //{
        //    Debug.Log(diff.magnitude);
        //    targetPos = playerControl.transform.position + new Vector3(0f, pullHeight, 0f);
        //    diff = targetPos - rb.transform.position;
        //    rb.velocity = diff.normalized * pullSpeed;
        //    yield return new WaitForEndOfFrame();
        //}
        
        //rb.transform.position = playerControl.transform.position + new Vector3(0f, pullHeight, 0f);
        
        target = rb;
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mouseWorldPos - (Vector2)target.transform.position).normalized;
        target.gameObject.GetComponent<Rigidbody2D>().velocity = dir * throwSpeed;
        pulled = false;
        target.transform.parent = null;
        

        //触发
        TriggerInstanceEvent(rb.GetComponent<Thing>());


        yield return new WaitForSeconds(0.15f);
        rb.gameObject.layer = layer;
        
        yield return new WaitForSeconds(0.2f);
        //
        //target.GetComponent<BoxCollider2D>().enabled = true;
    }

    public Vector3 PlotTrajectoryAtTime(Vector3 start, Vector3 startVelocity, float time)
    {
        return start + startVelocity * time + Physics.gravity * playerBody.gravityScale * time * time * 0.5f;
    }

    public Vector3[] PlotTrajectory(Vector3 start, Vector3 startVelocity, float fTimeStep, int stepCounts)
    {
        float maxTime = fTimeStep * stepCounts;
        Vector3[] results = new Vector3[stepCounts];
        Vector3 prev = start;
        for (int i = 1; ; i++)
        {
            float t = fTimeStep * i;
            if (t >= maxTime) break;
            Vector3 pos = PlotTrajectoryAtTime(start, startVelocity, t);
            if (Physics.Linecast(prev, pos)) break;
            prev = pos;
            results[i - 1] = pos;
        }
        return results;
    }
}
