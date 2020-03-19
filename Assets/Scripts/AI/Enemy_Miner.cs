using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Miner : Enemy_Shooter_Aim
{
    public float explodeDelay;
    public float speed;
    bool busy;
    public float explosionDelay;
    public float explosionRadius;
    public GameObject explosionAreaIndicator;
    public int damage;
    private void Start()
    {
        base.Start();
    }
    public void SelfDestroy()
    {
        StartCoroutine(DoRun());
    }

    IEnumerator DoRun()
    {
        float timer = 0f;
        while (timer < explodeDelay)
        {
            timer += Time.deltaTime;
            if (player.transform.position.x > transform.position.x)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(Explode());
    }
    public IEnumerator Explode()
    {
        if (busy) yield break;
        busy = true;
        GetComponent<SpriteRenderer>().color = Color.black;
        yield return new WaitForSeconds(explosionDelay);


        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        GameObject area = Instantiate(explosionAreaIndicator, transform.position, Quaternion.identity);
        area.transform.parent = null;
        area.GetComponent<SpriteRenderer>().size = new Vector2(explosionRadius * 2, explosionRadius * 2);
        Destroy(area, 0.1f);
        foreach (var col in cols)
        {
            if (col.CompareTag("player"))
            {
                col.GetComponent<PlayerControl1>().Die();
            }
            if (col.CompareTag("thing") && col.GetComponent<Enemy>())
            {
                col.GetComponent<Enemy>().TakeDamage(damage);
            }
            if (col.CompareTag("thing"))
                col.GetComponent<Thing>().TriggerMethod?.Invoke();
        }
        thing.Die();
    }
}
