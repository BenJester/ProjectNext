using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explosionRadius;
    public int damage = 1;
    bool active;
    public GameObject particle;

    private void Start()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        yield return new WaitForSeconds(0.2f);
        active = true;
    }

    void OnCollisionStay2D(Collision2D col)
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
            if (col.CompareTag("thing") && col.GetComponent<Enemy>())
            {
                col.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
        GameObject par = Instantiate(particle, transform.position, Quaternion.identity);
        active = false;
        GetComponent<Thing>().Die();

        //GameObject.Destroy(gameObject);
    }
}
