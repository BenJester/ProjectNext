using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;
public class Key : MonoBehaviour
{
    public bool activated;
    SpriteRenderer sr;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("player") && !activated)
        {
            activated = true;
            sr.enabled = false;
            ProCamera2DShake.Instance.Shake(0.2f, new Vector2(50f, 50f));
        }
    }
}
