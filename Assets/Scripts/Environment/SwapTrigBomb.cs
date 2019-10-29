using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapTrigBomb : MonoBehaviour
{
    public float explosionRadius;
    public int damage = 1;
    //bool active;
    public GameObject particle;
    private bool m_bSwap;
    private void Start()
    {
        //StartCoroutine(Init());
    }

    private void OnEnable()
    {
        Thing _thing = GetComponent<Thing>();
        if (_thing != null)
        {
            _thing.RegisteSwap(_swapThing);
        }
    }
    private void OnDisable()
    {
        Thing _thing = GetComponent<Thing>();
        if (_thing != null)
        {
            _thing.UnregisteSwap(_swapThing);
        }
    }

    private void _swapThing()
    {
        m_bSwap = true;
    }

    //IEnumerator Init()
    //{
    //    yield return new WaitForSeconds(0.2f);
    //    active = true;
    //}

    void OnCollisionStay2D(Collision2D col)
    {
        Explode();
    }

    void Explode()
    {
        //if (!active) return;
        if(m_bSwap == true)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            bool bExplode = false;
            foreach (var col in cols)
            {
                if (col.CompareTag("player"))
                {
                    col.GetComponent<PlayerControl1>().Die();
                    bExplode = true;
                }
                if (col.CompareTag("thing") && col.GetComponent<Enemy>())
                {
                    col.GetComponent<Enemy>().TakeDamage(damage);
                    bExplode = true;
                }
            }
            if(bExplode == true)
            {
                GameObject par = Instantiate(particle, transform.position, Quaternion.identity);
                //active = false;
                GetComponent<Thing>().Die();
            }
        }
    }
}
