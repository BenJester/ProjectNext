using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBossAttack : StateMachineBehaviour
{
    public Transform laserEmitPos;
    public float riseSpeed;
    public int stage1LaserNum;
    public int stage2LaserNum;
    public int stage3LaserNum;
    public LaserRotation laser1;
    public LaserRotation laser2;
    public LaserRotation laser3;
    public LaserRotation currLaser;
    public float laserRotateSpeed;
    public float laserPreload;
    public float laserDur;
    Rigidbody2D rb;
    IEnumerator Rise(Animator animator)
    {
        rb.velocity = new Vector2(0f, riseSpeed);
        while (Vector3.Distance(animator.transform.position, laserEmitPos.position) > 15f)
        {
            yield return new WaitForEndOfFrame();
        }
        rb.velocity = Vector2.zero;
    }

    IEnumerator Laser()
    {
        float timer = 0f;
        while (timer < laserDur)
        {
            currLaser
        }
        yield break;
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        if (animator.GetInteger("Stage") == 1)
        {
            currLaser = laser1;
        }
        else if (animator.GetInteger("Stage") == 2)
        {
            currLaser = laser2;
        }
        else if(animator.GetInteger("Stage") == 3)
        {
            currLaser = laser3;
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
