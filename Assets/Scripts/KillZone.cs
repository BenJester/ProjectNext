using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Kill()
    {
        
        StartCoroutine(DelayedKill());
    }
    bool busy;
    IEnumerator DelayedKill()
    {
        if (busy) yield break;
        busy = true;
        Color color = sr.color;
        sr.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        active = true;
        yield return new WaitForSeconds(0.5f);
        active = false;
        sr.color = color;
    }
    bool active;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (active && collision.CompareTag("player"))
        {
            PlayerControl1.Instance.PlayerDieImmediately();
        }
    }
}
