using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBox : MonoBehaviour
{


    public int damage;
    // Start is called before the first frame update
   

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            collision.GetComponent<PlayerControl1>().Die();
        }
        if (collision.CompareTag("thing") && collision.GetComponent<Enemy>())
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);
        }
        if (collision.CompareTag("thing") && collision.GetComponent<Thing>().TriggerMethod != null)
            collision.GetComponent<Thing>().TriggerMethod.Invoke();
    }

}
