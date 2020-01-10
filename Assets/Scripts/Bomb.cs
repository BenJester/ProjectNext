using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explosionRadius;
    public int damage = 1;
    bool active;
    public GameObject particle;
    private int m_nCountsExplode;
    private void Start()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        yield return new WaitForSeconds(0.2f);
        active = true;
    }

    //void OnCollisionStay2D(Collision2D col)
    //{
    //    Explode();
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Explode();
        if(active == true)
        {
            if (collision.gameObject.CompareTag("player"))
            {
                collision.gameObject.GetComponent<PlayerControl1>().Die();
            }
            if (collision.gameObject.CompareTag("thing") && collision.gameObject.GetComponent<Enemy>())
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }
            GameObject par = Instantiate(particle, transform.position, Quaternion.identity);
            active = false;
            GetComponent<Thing>().Die();

            GameObject.Destroy(gameObject);
        }
    }

    void Explode()
    {
        //if (!active)
        //{
        //    //m_nCountsExplode++;
        //    //if(m_nCountsExplode == 2)
        //    //{
        //    //    Debug.Assert(false);
        //    //}
        //    return;
        //}
        //Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        //foreach (var col in cols)
        //{
        //    if (col.CompareTag("player"))
        //    {
        //        col.GetComponent<PlayerControl1>().Die();
        //    }
        //    if (col.CompareTag("thing") && col.GetComponent<Enemy>())
        //    {
        //        col.GetComponent<Enemy>().TakeDamage(damage);
        //    }
        //}

        //GameObject par = Instantiate(particle, transform.position, Quaternion.identity);
        //active = false;
        //GetComponent<Thing>().Die();

        //GameObject.Destroy(gameObject);
    }
}
