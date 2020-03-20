using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ti_TimerBomb : MonoBehaviour,TriggerItem_Base
{
    [Header("炸弹被点燃")]
    public bool isTrigger = false;

    [Header("炸弹被点燃后爆炸的时间")]

    public float triggerTime = 3;
    public float triggerDelay;
    public float explosionRadius;
    public int damage = 1;
    private float tempTimer;
    private float _triggerTime;

    public Text timeLeft;

    public GameObject areaIndicator;
    Thing thing;
    bool triggered;
    void Start()
    {
        thing = GetComponent<Thing>();
        thing.OnDie += Explode;
        timeLeft.text = triggerTime.ToString("F1");
    }

    void Update()
    {
         if(isTrigger && tempTimer <= Time.time){
            Explode();
            isTrigger = false;
            //Destroy(gameObject);
        }

        if (triggered) {
            float time = (triggerTime - (Time.time - _triggerTime));
            time = Mathf.Clamp(time, 0, triggerTime);
            timeLeft.text = time.ToString("F1");
        }
    }

    public  void HandleKickTrigger(){
        if(!isTrigger) TriggerBomb();
        
    } 
    public  void HandleSwapTrigger(){
        if(!isTrigger) TriggerBomb();
        
    } 

    void TriggerBomb(){
        isTrigger=true;
        GetComponent<SpriteRenderer>().color = Color.red;
        tempTimer = Time.time + triggerTime;
    }
    

    IEnumerator DoExplode()
    {
        GetComponent<SpriteRenderer>().color = Color.red;


        _triggerTime = Time.time;
        yield return new WaitForSeconds(triggerTime);
        
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        GameObject area = Instantiate(areaIndicator, transform.position, Quaternion.identity);
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
            if (col.CompareTag("thing") && col.GetComponent<Thing>().TriggerMethod != null)
                col.GetComponent<Thing>().TriggerMethod.Invoke();
        }
        
        if (!thing.dead)
            thing.Die();
        Destroy(gameObject);
    }

    public void Explode()
    {
        if (triggered) return;
        triggered = true;
        StartCoroutine(DoExplode());
    }
}
