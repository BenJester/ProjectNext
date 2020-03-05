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
    public GameObject laser1;
    public GameObject laser2;
    public GameObject laser3;
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

        }
        yield break;
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
