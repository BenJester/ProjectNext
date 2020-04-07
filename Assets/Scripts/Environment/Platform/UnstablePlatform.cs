using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstablePlatform : MonoBehaviour
{
    public float disappearDelay;
    public float respawnDelay;
    public float blinkInterval;
    public float blinkAlpha;
    float originalAlpha;
    BoxCollider2D box;
    SpriteRenderer sr;
    bool busy;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("player") && !busy)
        {
            Fall();
        }
    }

    void Fall()
    {
        busy = true;
        StartCoroutine(DelayedFall());
        StartCoroutine(Blink());
        StartCoroutine(Respawn());
    }
    IEnumerator Blink()
    {
        float timer = 0f;
        while (timer < disappearDelay)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, blinkAlpha);
            yield return new WaitForSeconds(blinkInterval);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, originalAlpha);
            yield return new WaitForSeconds(blinkInterval);
            timer += 2 * blinkInterval;
            
        }
    }
    IEnumerator DelayedFall()
    {
        yield return new WaitForSeconds(disappearDelay);
        box.enabled = false;
        sr.enabled = false;
    }
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, originalAlpha);
        box.enabled = true;
        sr.enabled = true;
        busy = false;
    }

    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        originalAlpha = sr.color.a;
    }

    void Update()
    {
        
    }
}
