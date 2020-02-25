using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("thing"))
            collision.gameObject.GetComponent<Thing>().TriggerMethod?.Invoke();
    }
}
