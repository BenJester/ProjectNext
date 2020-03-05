using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // Start is called before the first frame update
    int damage = 1;
    public SpriteRenderer sr;
    public bool damageOn;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!damageOn) return;
        if (col.CompareTag("player"))
        {
            col.GetComponent<PlayerControl1>().Die();
        }

        if (col.CompareTag("thing") && col.GetComponent<Thing>().TriggerMethod != null)
            col.GetComponent<Thing>().TriggerMethod.Invoke();
    }
}
