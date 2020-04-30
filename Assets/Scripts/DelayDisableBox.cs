using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDisableBox : MonoBehaviour
{
    public float delay;
    BoxCollider2D box;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        //StartCoroutine(DelayDisable());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        box.enabled = false;
        sr.enabled = false;
    }

    IEnumerator DelayDisable()
    {
        yield return new WaitForSeconds(delay);
        box.enabled = false;
        sr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
