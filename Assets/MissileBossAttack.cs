using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBossAttack : StateMachineBehaviour
{
    public float riseSpeed;

    public LaserRotation currLaser;
    public float laserWarningTime;
    public float laserRotateSpeed;
    public float laserPreload;
    public float laserDur;
    public float laserPostload;
    Rigidbody2D rb;
    Thing thing;

    public int times;
    public float interval;
    Transform transform;
    Transform player;
    Vector2 direction;
    public float throwSpeed;
    Animator animator;
    MissileBoss boss;
    LaserRotation actualLaser;
    IEnumerator Rise()
    {
        Debug.Log("rise0");
        rb.velocity = (boss.laserEmitPos.position - animator.transform.position).normalized * riseSpeed;
        while (Vector3.Distance(animator.transform.position, boss.laserEmitPos.position) > 15f)
        {
            yield return new WaitForEndOfFrame();
        }
        rb.velocity = Vector2.zero;
        PlayerControl1.Instance.StartCoroutine(Laser());
    }
    IEnumerator Descend()
    {
        
        Vector3 target = Vector3.zero;
        if (animator.GetInteger("Stage") == 1)
        {
            target = boss.stage1MissilePos.position;
        }
        else if (animator.GetInteger("Stage") == 2)
        {
            target = boss.stage2MissilePos.position;
        }
        else if (animator.GetInteger("Stage") == 3)
        {
            target = boss.stage3MissilePos.position;
        }
        rb.velocity = (target - transform.position).normalized * riseSpeed;
        while (Vector3.Distance(transform.position, target) > 15f)
        {
            yield return new WaitForEndOfFrame();
        }
        rb.velocity = Vector2.zero;
        //PlayerControl1.Instance.StartCoroutine(Laser());

    }
    IEnumerator StartMisselThrow()
    {
        int i = 0;
        while (i < times)
        {
            ThrowMissel();
            yield return new WaitForSeconds(interval);
            i++;
        }
        PlayerControl1.Instance.StartCoroutine(Descend());
    }

    void ThrowMissel()
    {
        if (thing.dead)
            return;

        direction = Vector3.up;// (PlayerControl1.Instance.transform.position - transform.position).normalized;
        GameObject bombTemp = Instantiate(boss.missel, (Vector2)transform.position + direction * 200, Quaternion.identity);
        bombTemp.GetComponent<Rigidbody2D>().velocity = direction * throwSpeed;
        Ti_Missel bombTi = bombTemp.GetComponent<Ti_Missel>();
        bombTi.target = player;
        bombTi.transform.localRotation = Quaternion.Euler(0, 0, -Dash.AngleBetween(Vector2.up, direction));
        bombTi.isTrigger = true;

    }

    IEnumerator Laser()
    {
        actualLaser = currLaser;
        currLaser.Warning();
        yield return new WaitForSeconds(laserWarningTime);
        currLaser.Damage();
        Debug.Log("laser activate");
        yield return new WaitForSeconds(laserDur);
        currLaser.Close();
        yield return new WaitForSeconds(laserPostload);
        PlayerControl1.Instance.StartCoroutine(StartMisselThrow());
        yield break;
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<MissileBoss>();
        this.animator = animator;
        player = PlayerControl1.Instance.transform;
        transform = animator.transform;
        thing = animator.GetComponent<Thing>();
        rb = animator.GetComponent<Rigidbody2D>();
        if (animator.GetInteger("Stage") == 1)
        {
            currLaser = boss.laser1;
            times = 1;
        }
        else if (animator.GetInteger("Stage") == 2)
        {
            currLaser = boss.laser2;
            times = 1;
        }
        else if(animator.GetInteger("Stage") == 3)
        {
            currLaser = boss.laser3;
            times = 3;
        }
        PlayerControl1.Instance.StartCoroutine(Rise());
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

}
