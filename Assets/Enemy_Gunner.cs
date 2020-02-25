using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Gunner : Enemy, TriggerItem_Base
{





    [Space]
    //TODO:还没有加入动画
    public PlayerState state;
    public bool hasWeapon = true;
    public Ti_Gun Gun;
    public float aimTime = 0.5f;
    public float lockTime = 30;
    public float shootTime = 10;
    public float reloadTime = 120;
    public enum PlayerState
    {
        idle = 0,
        aiming = 1,
        locking = 2,
        shoot = 3,
        reloading = 4,
        shock = 5,

        dead = 6,

        //TODO: 被缴械之后应该会变成普通的敌人
    }
    //Extra
    [System.NonSerialized] public float[] stateActiveFrames = new float[5] { 0, 0, 0, 0, 0 };
    [System.NonSerialized] public float[] stateInactiveFrames = new float[5] { 0, 0, 0, 0, 0 };

    //预计状态所对应的帧数
    public int[] stateActiveTimeCounts;
    //通过预计帧数计算可能的时间段值
    public bool faceRight = true;
    public float distance = 1000;
    private Vector2 direction;
    public Transform player;
    public LineRenderer lr;

    private void Awake()
    {

        lr = GetComponent<LineRenderer>();
        Gun = GetComponentInChildren<Ti_Gun>();
    }
    void Start()
    {
        base.Start();
        //Gun.transform.SetParent(null);
        if (player == null) player = PlayerControl1.Instance.transform;
        if (faceRight) direction = transform.right;
        else direction = -transform.right;
        //TODO: 枪械转向还没做

    }

    public void HandleKickTrigger()
    {

    }

    public void HandleSwapTrigger()
    {
        if (hasWeapon)
        {
            hasWeapon = false;
            Gun.transform.SetParent(null);
            UseGun();
        }


    }

    void Update()
    {


        StateUpdate();
        if (thing.dead)
        {
            lr.enabled = false;
            return;
        }
        if (Gun.transform.parent == null) hasWeapon = false;

        switch (state)
        {
            case PlayerState.idle:
                if (stateActiveFrames[(int)PlayerState.idle] == 0) lr.enabled = false;
                break;
            case PlayerState.aiming:
                if (stateActiveFrames[(int)PlayerState.aiming] == 0) lr.enabled = true;

                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, (Vector3)direction * distance + transform.position);
                lr.startColor = Color.green;
                lr.endColor = Color.green;
                break;

            case PlayerState.locking:
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, (Vector3)direction * distance + transform.position);
                lr.startColor = Color.yellow;
                lr.endColor = Color.yellow;
                break;

            case PlayerState.shoot:
                if (stateActiveFrames[(int)PlayerState.shoot] == 0)
                {
                    lr.enabled = false;
                    UseGun();
                }

                break;

            case PlayerState.reloading:
                if (stateActiveFrames[(int)PlayerState.reloading] == 0) lr.enabled = false;
                break;
            case PlayerState.dead:
                return;

        }

        TimeUpdate();
    }

    private void TimeUpdate()
    {
        for (int i = 0; i < stateInactiveFrames.Length; i++)
        {
            stateInactiveFrames[i] = ((int)state == i) ? 0 : stateInactiveFrames[i] += Time.deltaTime;
        }

        for (int i = 0; i < stateActiveFrames.Length; i++)
        {
            stateActiveFrames[i] = ((int)state == i) ? stateActiveFrames[i] + Time.deltaTime : 0;
        }
    }

    private void StateUpdate()
    {

        if (!hasWeapon)
        {
            state = PlayerState.shock;
        }
        if (state == PlayerState.idle)
        {
            if (health == 0)
            {
                state = PlayerState.dead;
            }
            else if (CheckPlayerInPlainSight()) state = PlayerState.aiming;
        }
        else if (state == PlayerState.aiming)
        {
            if (stateActiveFrames[(int)PlayerState.aiming] > aimTime)
            {
                state = PlayerState.locking;
            }
        }
        else if (state == PlayerState.locking)
        {
            if (stateActiveFrames[(int)PlayerState.locking] > lockTime)
            {
                state = PlayerState.shoot;
            }
        }
        else if (state == PlayerState.shoot)
        {
            if (stateActiveFrames[(int)PlayerState.shoot] > shootTime)
            {
                state = PlayerState.reloading;
            }
        }
        else if (state == PlayerState.reloading)
        {
            if (stateActiveFrames[(int)PlayerState.reloading] > reloadTime)
            {
                state = PlayerState.idle;
            }
        }
    }





    void UseGun()
    {
        if (thing.dead)
            return;

        Gun.StartCoroutine(Gun.Shoot());


    }
    public bool CheckPlayerInPlainSight()
    {

        RaycastHit2D[] hits = Physics2D.RaycastAll((Vector2)transform.position - Vector2.up * 10, direction.normalized, distance, (1 << 10) | (1 << 8) | (1 << 9) | (1 << 12));
        RaycastHit2D hitNear;
        if (hits.Length >= 3)
        {
            hitNear = hits[2];
            if (hitNear.collider.tag == "player") return true;
            else return false;
        }
        else return false;
    }
    // public bool CheckPlayerInSight () {
    // 	RaycastHit2D[] hits = Physics2D.RaycastAll (transform.position, (player.position - transform.position).normalized, distance, (1 << 10) | (1 << 8) | (1 << 9) | (1 << 12));
    // 	RaycastHit2D hitNear;
    // 	if (hits.Length >= 3) {
    // 		hitNear = hits[2];
    // 		if (hitNear.collider.tag == "player") return true;
    // 		else return false;
    // 	} else return false;
    // }

    private void OnDrawGizmos()
    {
        //if (player != null) Gizmos.DrawLine(transform.position, (player.position - transform.position).normalized * distance + transform.position);
    }
    public static float AngleBetween(Vector2 vectorA, Vector2 vectorB)
    {
        float angle = Vector2.Angle(vectorA, vectorB);
        Vector3 cross = Vector3.Cross(vectorA, vectorB);

        if (cross.z > 0)
        {
            angle = 360 - angle;
        }

        return angle;
    }
}