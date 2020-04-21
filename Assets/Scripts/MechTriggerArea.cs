using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MechTriggerArea : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isTrigger = false;
    public Mech_base triggerMech;
    public float timeDelayToTrigger;
    float timeTemp;
    SpriteRenderer spr;

    public Text time;

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

    // Update is called once per frame
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
    }
}
