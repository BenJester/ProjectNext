using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public float explosionDelay;
    public float explosionRadius;
    public GameObject explosionAreaIndicator;
    public int damage;
    bool busy;
    public AudioClip clip;
    AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoExplode()
    {
        StartCoroutine(Explode());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player") || collision.CompareTag("thing") || collision.CompareTag("bullet"))
        {
            StartCoroutine(Explode());
        }
    }

    public IEnumerator Explode()
    {
        if (busy) yield break;
        busy = true;
        GetComponent<SpriteRenderer>().color = Color.black;
        source.PlayOneShot(clip);
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
        Destroy(gameObject, 0.1f);
    }
}
