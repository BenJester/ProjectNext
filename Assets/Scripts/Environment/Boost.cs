using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;
    public float disableAirControlDur;
    PlayerControl1 pc;
    void Start()
    {
        pc = PlayerControl1.Instance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Thing>() != null)
        {
            StartCoroutine(DisableAirControl(collision));
            
        }
    }
    IEnumerator DisableAirControl(Collision2D collision)
    {
        if (collision.collider.CompareTag("player"))
        {
            GetComponent<Thing>().Die();
            pc.disableAirControl = true;
            yield return new WaitForSeconds(0.05f);
            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(xSpeed, ySpeed);
            yield return new WaitForSeconds(disableAirControlDur);
            pc.disableAirControl = false;
            
        }
        else
        {
            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(xSpeed, ySpeed);
        }

        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
