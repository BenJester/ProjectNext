using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Com.LuisPedroFonseca.ProCamera2D;

public class MechTriggerArea : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isTrigger = false;
    public Mech_base triggerMech;
    public float timeDelayToTrigger;
    float timeTemp;
    SpriteRenderer spr;

    public Text time;
    public bool activated = false;
    public UnityEvent triggerEvent;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        timeTemp = timeDelayToTrigger;
        time.text = timeTemp.ToString("F1");
    }

    void Update()
    {
        if (isTrigger) {
            time.text = timeTemp.ToString("F1");
            timeTemp -= Time.deltaTime;
            timeTemp = Mathf.Clamp(timeTemp, 0, timeDelayToTrigger);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTrigger && collision.tag == "player") {
            spr.color = Color.yellow;
            isTrigger = true;
            StartCoroutine(StartTrigger(timeDelayToTrigger));
        }
    }

    IEnumerator StartTrigger(float delay) {
        yield return new WaitForSeconds(delay);
        triggerMech?.DoOnce();
        spr.color = Color.red;
        triggerEvent?.Invoke();
        activated = true;
        ProCamera2DShake.Instance.Shake(0.2f, new Vector2(100f, 100f));
    }
}
