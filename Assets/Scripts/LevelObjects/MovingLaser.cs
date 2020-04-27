using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingLaser : MonoBehaviour
{
    public bool active = true;


   
    Vector2 dir;
    public float shootDistance;
    public Transform moveTarget;
    public Transform moveStart;
    public int damage = 1;
    Vector3 start;
    Vector3 finish;
    Vector3 currTarget;

    public float speed;
    LineRenderer lr;
    public LayerMask hitLayer;
    Rigidbody2D rb;
    SpriteRenderer sr;



    [Space]
    [Header("开关切换的参数")]
    public bool isIntervalsLaser = false;
    public bool isDoubleIntervalLaser = false;
    public bool isCountDownLaser = false;
    public float interval;
    public float inactiveInterval;
    public float offset;
    public PhysicalButton button;
    public MechTriggerArea triggerArea;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        start = moveStart.position;
        finish = moveTarget.position;
        currTarget = finish;

        dir = transform.up;

        if(isIntervalsLaser) StartCoroutine(IntervalLaser());
        else if (isDoubleIntervalLaser) StartCoroutine(DoubleIntervalLaser());
    }

    void FixedUpdate()
    {
        HandleColor();
        HandleRay();
        HandleMovement();
        HandleButton();
        HandleTrigger();
    }

    void HandleTrigger()
    {
        if (triggerArea == null || !triggerArea.activated) return;
        active = true;
        

    }

    IEnumerator DelayedSwitch()
    {
        yield return new WaitForSeconds(0.1f);
        if (button.state != ClickState.IsClick)
            active = false;
        else
            active = true;
    }

    ClickState prev;

    void HandleButton()
    {
        if (button == null) return;
        if (prev != button.state)
            StartCoroutine(DelayedSwitch());
        prev = button.state;
    }

    void HandleColor(){
        if(active) sr.color = Color.red;
        else sr.color = Color.yellow; 
    }

    void HandleRay()
    {
        if (!active)
        {
            lr.enabled = false;

            return;
        }
        
        lr.enabled = true;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, shootDistance, hitLayer);
        lr.SetPosition(1, hit.point);
        lr.SetPosition(0, transform.position);
        Collider2D col = hit.collider;
        if (col.CompareTag("player"))
        {
            col.GetComponent<PlayerControl1>().Die();
        }
        if (col.CompareTag("thing") && col.GetComponent<Enemy>())
        {
            col.GetComponent<Enemy>().TakeDamage(damage);
        }
        if (col.CompareTag("thing") && col.GetComponent<Thing>().TriggerMethod != null)
            col.GetComponent<Thing>().TriggerMethod.Invoke();
    }

    void HandleMovement()
    {
        if (speed == 0f) return;
        rb.velocity = (currTarget - transform.position).normalized * speed;
        if (Vector3.Distance(transform.position, currTarget) < speed * 0.025f)
        {
            transform.position = currTarget;
            currTarget = currTarget == start ? finish : start;
        }
    }

    IEnumerator IntervalLaser(){
        yield return new WaitForSeconds(offset);
        while(true){
            active = !active;
            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator DoubleIntervalLaser()
    {
        yield return new WaitForSeconds(offset);
        while (true)
        {
            active = true;
            yield return new WaitForSeconds(interval);
            active = false;
            yield return new WaitForSeconds(inactiveInterval);
        }
    }

    public void SetActive(float spe) {
        speed = spe;
        active = true;
    }
}
