using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBoss : Enemy
{
    public Transform laserEmitPos;

    public LaserRotation laser1;
    public LaserRotation laser2;
    public LaserRotation laser3;

    public Transform stage1MissilePos;
    public Transform stage2MissilePos;
    public Transform stage3MissilePos;

    public GameObject missel;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 4)
        {
            animator.SetInteger("Stage", 2);
        }
        if (health == 2)
        {
            animator.SetInteger("Stage", 3);
        }
    }
}
