using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;
public class MovablePillar : MonoBehaviour
{
    // Start is called before the first frame update
    public float delay;
    public float flickerInterval;
    public float stompSpeed;
    public float stompDistance;
    public float stompPostDur;
    public float stompReturnSpeed;
    Vector3 originalPos;
    SpriteRenderer sr;
    Rigidbody2D rb;
    public MovablePillarKill killZone;
    void Start()
    {
        if (down)
            originalPos = transform.position + Vector3.up * stompDistance;
        else
            originalPos = transform.position;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    public bool down;
    public bool busy;
    public bool atTop;
    // Update is called once per frame
    void Update()
    {
        if (!down && Mathf.Abs(transform.position.y - originalPos.y) > 15f && !atTop && !busy)
        {
            rb.velocity = new Vector2(0f, stompReturnSpeed);
        }
        if (Mathf.Abs(transform.position.y - originalPos.y) < 5f && !atTop)
        {
            atTop = true;
            rb.velocity = Vector2.zero;
        }
        if (down && !busy && atTop)
        {
            StartCoroutine(DoStomp());
        }
    }

    public void Stomp()
    {
        down = true;
    }
    public void Return()
    {
        down = false;
    }
    IEnumerator DoReturn()
    {

        float timer = 0f;
        yield return new WaitForSeconds(stompPostDur);
        float dur = stompDistance / stompReturnSpeed;
        rb.velocity = new Vector2(0f, stompReturnSpeed);
        while (timer < dur)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }
        rb.velocity = Vector2.zero;
        yield break;
    }
    IEnumerator DoStomp()
    {
        
        if (busy) yield break;
        busy = true;
        float timer = 0f;
        
        while (timer < delay)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(flickerInterval);
            timer += flickerInterval;
            sr.color = Color.white;
            yield return new WaitForSeconds(flickerInterval);
            timer += flickerInterval;
        }
        sr.color = Color.white;
        killZone.active = true;
        timer = 0f;
        float dur = stompDistance / stompSpeed;
        rb.velocity = new Vector2(0f, -stompSpeed);
        while (timer < dur)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }
        ProCamera2DShake.Instance.Shake(0.2f, new Vector2(100f, 100f));

        rb.velocity = Vector2.zero;
        atTop = false;
        yield return new WaitForSeconds(0.1f);
        killZone.active = false;
        busy = false;
    }
}
