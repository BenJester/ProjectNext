using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardThrow : Skill
{
    public GameObject Card;
    public float throwOffset;
    public float throwSpeed;

    private void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            StartCoroutine(DoKick(Card.GetComponent<Rigidbody2D>()));
        }
    }

    IEnumerator DoKick(Rigidbody2D rb)
    {
        Vector2 dir;

        int layer = rb.gameObject.layer;
        //rb.gameObject.layer = 19;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dir = (mouseWorldPos - (Vector2)playerControl.transform.position).normalized;

        rb.transform.position = playerControl.transform.position + (Vector3)dir * throwOffset;
        rb.gameObject.GetComponent<Rigidbody2D>().velocity = dir * throwSpeed;


        yield return new WaitForSeconds(0.15f);
        //rb.gameObject.layer = layer;
    }
}
