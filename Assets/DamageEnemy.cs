using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public bool damagePlayer=false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Thing>().type==Ben.Type.enemy) {
            collision.GetComponent<Enemy>().TakeDamage(1);
        
        }
        if (damagePlayer && collision.tag == "player") {
            PlayerControl1.Instance.Die();
        }
    }
}
