using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

public class Boost : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;
    public float disableAirControlDur;
    PlayerControl1 pc;
    BoxCollider2D box;
    SpriteRenderer sr;
    Rigidbody2D mrb;
    bool originallyHasShield;
    public float delay;
    void Start()
    {
        pc = PlayerControl1.Instance;
        sr = GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
        mrb = GetComponent<Rigidbody2D>();
        originalPos = transform.position;
        originallyHasShield = GetComponent<Thing>().hasShield;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Thing>() != null)
        {
            StartCoroutine(DisableAirControl(collision));
            
        }
    }
    IEnumerator DisableAirControl(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            yield return new WaitForSeconds(delay);
            ProCamera2DShake.Instance.Shake(0.2f, new Vector2(80f, 80f));
            sr.enabled = false;
            box.enabled = false;
            pc.disableAirControl = true;
            pc.GetComponent<AirJump>().charge = pc.GetComponent<AirJump>().maxCharge;
            yield return new WaitForSeconds(0.05f);
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(xSpeed, ySpeed);
            yield return new WaitForSeconds(disableAirControlDur);
            pc.disableAirControl = false;
            mrb.velocity = Vector2.zero;
            GetComponent<Thing>().dead = true;
        }
        else
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(xSpeed, ySpeed);
            ProCamera2DShake.Instance.Shake(0.2f, new Vector2(50f, 50f));
            sr.enabled = false;
            box.enabled = false;
            GetComponent<Thing>().dead = true;
        }

        
    }
    bool busy;
    public float reviveDelay;
    Vector3 originalPos;
    IEnumerator DelayedRevive()
    {
        busy = true;
        mrb.velocity = Vector2.zero;
        
        yield return new WaitForSeconds(reviveDelay);
        sr.enabled = true;
        box.enabled = true;
        GetComponent<Thing>().hasShield = originallyHasShield;
        GetComponent<Thing>().dead = false;
        transform.position = originalPos;
        busy = false;
        mrb.velocity = Vector2.zero;
    }
    // Update is called once per frame
    void Update()
    {
        if (box.enabled == false && PlayerControl1.Instance.isTouchingGround && !busy)
        {
            StartCoroutine(DelayedRevive());
        }
    }
}
