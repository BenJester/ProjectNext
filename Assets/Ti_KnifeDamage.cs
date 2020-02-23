using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ti_KnifeDamage : MonoBehaviour
{
    private Rigidbody2D my_Rb;
    public bool isFly;
    public int damage;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        my_Rb = GetComponentInParent<Rigidbody2D>();
    }

    void Update()
    {
       isFly = my_Rb.velocity.magnitude>1f;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("player"))
        {
            //other.transform.GetComponent<PlayerControl1>().Die();
        }
        if (other.transform.CompareTag("thing") && other.transform.GetComponent<Enemy>() && isFly)
        {
            other.transform.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
