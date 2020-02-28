using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ben;
using DG.Tweening;
public class EnemyBullet_Transable_Forward : MonoBehaviour, TriggerItem_Base
{
    // Start is called before the first frame update

    public bool floorCollide = true;
    PlayerControl pc;
    void Start()
    {
        pc = GameObject.FindWithTag("player").GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {


    }
    public void HandleSwapTrigger()
    {
        transform.GetComponent<Rigidbody2D>().velocity *= 2;
        transform.DOScale(new Vector3(1.5f, 1.5f, 1),0.2f);
    }

    public void HandleKickTrigger()
    {

    }

    public void OnTriggerEnter2D(Collider2D col)
    {



        if (col.CompareTag("thing"))
        {
            if (col.GetComponent<Thing>().type == Type.enemy)
            {
                col.GetComponent<Enemy>().TakeDamage(2);
            }
            else if (col.GetComponent<Thing>().type != Type.box)
                col.GetComponent<Thing>().Die();
            col.GetComponent<Thing>().TriggerMethod?.Invoke();
            Deactivate();


        }
        else if (col.CompareTag("player"))
        {

            pc.Die();
            Deactivate();
        }
        else if (col.CompareTag("floor") && floorCollide)
        {
            Deactivate();
        }

    }

    public void Deactivate()
    {

        //active = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<TrailRenderer>().enabled = false;
        //rb.velocity = Vector2.zero;
    }
}
