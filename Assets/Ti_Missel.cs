using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

public class Ti_Missel : MonoBehaviour, TriggerItem_Base
{
    // Start is called before the first frame update
    public bool isAddVelocity=false;
    public bool isTrigger = false;
    public Transform target;

    public float explosionDelay;
    public float speedAcc = 1f;
    public float startSpeed = 0;
    public float maxSpeed;
    public float rotateSpeed = 200f;
    public float explosionRadius;
    public GameObject explosionAreaIndicator;
    public int damage;

    public GameObject explisionParticle;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTrigger)
        {
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rb.angularVelocity = -rotateAmount * rotateSpeed;
            if(isAddVelocity){
                rb.velocity += (Vector2)transform.up * speedAcc;
                rb.velocity = Mathf.Clamp(rb.velocity.magnitude, 0f, maxSpeed) * rb.velocity.normalized;
            }
            else{
                rb.velocity = (Vector2)transform.up * startSpeed;
                startSpeed = Mathf.Clamp(0f, startSpeed + speedAcc, maxSpeed);
            }
            
            
        }


    }
    public void Trigger()
    {
        isTrigger = true;
    }
    public void HandleKickTrigger()
    {

    }

    public void HandleSwapTrigger()
    {

    }

    public IEnumerator Explode()
    {
        GetComponent<SpriteRenderer>().color = Color.black;
        rb.velocity = Vector2.zero;
        isTrigger = false;
        yield return new WaitForSeconds(explosionDelay);

        ProCamera2DShake.Instance.Shake(0);


        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        GameObject smoke = Instantiate(explisionParticle,transform.position,Quaternion.identity);
        smoke.transform.parent=null;
        smoke.transform.localScale*=1.5f;
        Destroy(smoke,1f);

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
        GetComponent<Thing>().Die();
    }
}
