using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explosionRadius;
    bool active;

    private void Start()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        yield return new WaitForSeconds(0.5f);
        active = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Explode();
    }

    void Explode()
    {
        if (!active) return;
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var col in cols)
        {
            if (col.CompareTag("player"))
            {
                col.GetComponent<PlayerControl1>().Die();
            }
            if (col.CompareTag("thing"))
            {
                col.GetComponent<Thing>().Die();
            }
        }
        Destroy(gameObject);
    }
}
