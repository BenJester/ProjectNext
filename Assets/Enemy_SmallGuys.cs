using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SmallGuys : Enemy
{
    // Start is called before the first frame update


        [Header("简单会追逐玩家的敌人")]
    public float distance;
    public float escapeDistance;
    public bool isTracing;
    public float speed;
    public Transform target;


    [Header("老母亲")]
    public Enemy_MisselBoy wormMother;
    bool isDead = false;
     LineRenderer lr;
    

    
    void Start()
    {
        base.Start();
        target = PlayerControl1.Instance.GetComponent<Transform>();
        wormMother = GameObject.FindGameObjectWithTag("wormMother").GetComponent<Enemy_MisselBoy>();
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTracing && Vector2.Distance(transform.position, target.position) <= distance){
            isTracing = true;
        }

        if (isTracing && Vector2.Distance(transform.position, target.position) >= escapeDistance) {
            isTracing = false;
        }

        if (isTracing) FollowPlayer();
        else {
            return;
        }

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, wormMother.transform.position);

        if (health <= 0 && !isDead) {
            wormMother.TakeDamage(1);
            isDead = true;
            lr.enabled = false;
        }
        
    }

    public void FollowPlayer() {
        transform.position = transform.position + (target.position - transform.position).normalized * speed * Time.deltaTime; ;
    
    }

    

    
}
