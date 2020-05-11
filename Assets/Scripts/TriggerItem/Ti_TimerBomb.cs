using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ti_TimerBomb : MonoBehaviour,TriggerItem_Base
{
    public bool damageEnemy;
    public bool damagePlayer;
    public bool pushEnemy;
    public bool pushPlayer;
    [Header("炸弹被点燃")]
    public bool isTrigger = false;

    [Header("炸弹被点燃后爆炸的时间")]

    public float triggerTime = 3;
    public float triggerDelay;
    public float explosionRadius;
    public int damage = 1;
    private float tempTimer;
    private float _triggerTime;

    public float explodePreloadtime=0.1f;
    public GameObject exploParticle;

    public Text timeLeft;

    public GameObject areaIndicator;
    Thing thing;
    bool triggered;
    public AudioSource asr;

    public int explodeTime = 1;
    public float multiExplodeInterval = 0.5f;



    void Start()
    {
        thing = GetComponent<Thing>();
        thing.OnDie += Explode;
        timeLeft.text = triggerTime.ToString("F1");
        asr = GetComponent<AudioSource>();
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

    public float pushSpeed;
    public float disableAirControlTime;
    void PushPlayer(Collider2D col)
    {
        Vector2 dir = (col.transform.position - transform.position).normalized;
        PlayerControl1.Instance.rb.velocity = dir * pushSpeed;
        StartCoroutine(DisableAirControl());
    }

    IEnumerator DisableAirControl()
    {
        PlayerControl1.Instance.disableAirControl = true;
        yield return new WaitForSeconds(disableAirControlTime);
        PlayerControl1.Instance.disableAirControl = false;
    }

    IEnumerator DoExplode()
    {
        GetComponent<SpriteRenderer>().color = Color.red;

        
        _triggerTime = Time.time;
        yield return new WaitForSeconds(triggerTime- explodePreloadtime);


        if (exploParticle != null) {
            GameObject part1 = Instantiate(exploParticle, transform.position, Quaternion.identity);
            part1.GetComponent<SpriteRenderer>().size = new Vector2(explosionRadius * 2, explosionRadius * 2);
            Destroy(part1, 1f);
        }
        

        yield return new WaitForSeconds(explodePreloadtime);


        if(asr!=null) asr.Play();
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        GameObject area = Instantiate(areaIndicator, transform.position, Quaternion.identity);
        area.transform.parent = null;
        area.GetComponent<SpriteRenderer>().size = new Vector2(explosionRadius * 2, explosionRadius * 2);
        Destroy(area, 0.1f);
        foreach (var col in cols)
        {
            if (col.CompareTag("player"))
            {
                if (damagePlayer)
                    col.GetComponent<PlayerControl1>().Die();
                if (pushPlayer)
                    PushPlayer(col);
            }
            if (col.CompareTag("thing") && col.GetComponent<Enemy>())
            {
                if (damageEnemy)
                    col.GetComponent<Enemy>().TakeDamage(damage);
                if (pushEnemy)
                {
                    Vector2 dir = (col.transform.position - transform.position).normalized;
                    col.GetComponent<Rigidbody2D>().velocity = dir * pushSpeed;
                }

            }
            if (col.CompareTag("thing") && col.GetComponent<Thing>().TriggerMethod != null)
            {
                col.GetComponent<Thing>().TriggerMethod.Invoke();
                if (pushEnemy && col.GetComponent<Ti_TimerBomb>() == null)
                {
                    Vector2 dir = (col.transform.position - transform.position).normalized;
                    col.GetComponent<Rigidbody2D>().velocity = dir * pushSpeed;
                }
            }
                
        }
        
        


        explodeTime -= 1;
        if (explodeTime <= 0)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject, 0.3f);
            if (!thing.dead)
                thing.Die();
        }
        else {
            yield return new WaitForSeconds(multiExplodeInterval);
            StartCoroutine(DoExplode());
            
        }
        
    }

    public void Explode()
    {
        if (triggered) return;
        triggered = true;
        
        StartCoroutine(DoExplode());
    }

    public void Fall(){
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        Explode();


    }
}
