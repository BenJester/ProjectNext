
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy_Sentry : Enemy
{

    [Space]

    //TODO:还没有加入动画
    public PlayerState state;
    //public Text countDownText;
    public float aimTime = 0.5f;
    float shootTime = 0.1f;
    float countDown;
    public enum PlayerState
    {
        idle = 0,
        aim = 1,
        shoot = 2,
        dead = 3,

    }

    //Extra
    [System.NonSerialized] public float[] stateActiveFrames = new float[4] { 0, 0, 0, 0 };
    [System.NonSerialized] public float[] stateInactiveFrames = new float[4] { 0, 0, 0, 0 };

    //预计状态所对应的帧数
    public int[] stateActiveTimeCounts;
    //通过预计帧数计算可能的时间段值
    private float[] m_totalDeltaTime;
    private float m_fCurDeltaTime;


    public float distance = 1000;
    private Vector2 direction;

    private Animator animator;
    public Transform player;

    public LineRenderer lr;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        lr = GetComponent<LineRenderer>();
    }
    void Start()
    {
        base.Start();
        player = PlayerControl1.Instance.GetComponent<Transform>();
        lr.startWidth = 5;
        lr.endWidth = 5;
    }


    void Update()
    {


        StateUpdate();
        if (thing.dead)
        {
            lr.enabled = false;
            return;
        }

        switch (state)
        {
            case PlayerState.idle:
                if (stateActiveFrames[(int)PlayerState.idle] == 0) {
                    //countDown = aimTime;
                    //countDownText.text = countDown.ToString();
                    lr.enabled = false;
                }
                
           
                break;
            case PlayerState.aim:
                if (stateActiveFrames[(int)PlayerState.aim] == 0) {
                    lr.startWidth = 5;
                    lr.endWidth = 5;
                    lr.enabled = true;
                }


                //countDown = aimTime - stateActiveFrames[(int)PlayerState.aim];
                //countDownText.text = countDown.ToString();
                lr.startWidth = stateActiveFrames[(int)PlayerState.aim] * 5;
                lr.endWidth = stateActiveFrames[(int)PlayerState.aim] * 5;

                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, (Vector3)direction * distance + transform.position);
                lr.startColor = Color.yellow;
                lr.endColor = Color.yellow;

                direction = (player.position - transform.position).normalized;
                break;

            case PlayerState.shoot:
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, (Vector3)direction * distance + transform.position);
                lr.startWidth = 15;
                lr.endWidth = 15;
                lr.startColor = Color.red;
                lr.endColor = Color.red;
                LaserShoot();
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
        if (state == PlayerState.idle)
        {
            if (health == 0)
            {
                state = PlayerState.dead;
            }
            else if (CheckPlayer()) state = PlayerState.aim;
        }
        else if (state == PlayerState.aim)
        {
            if (stateActiveFrames[(int)PlayerState.aim] > aimTime)
            {
                state = PlayerState.shoot;
            }
            else if (!CheckPlayer()) {
                state = PlayerState.idle;
            }
        }
        else if (state == PlayerState.shoot)
        {
            if (stateActiveFrames[(int)PlayerState.shoot] > shootTime)
            {
                state = PlayerState.idle;
            }
        }
    }



    void LaserShoot()
    {
        
        if (thing.dead)
            return;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, distance, (1 << 10) | (1 << 8) | (1 << 9));
        RaycastHit2D hitNear;
        if (hits.Length >= 2)
        {
            hitNear = hits[1];
            if (hitNear.collider.tag == "player") hitNear.collider.GetComponent<PlayerControl>().Die();
            if (hitNear.collider.tag == "thing") hitNear.collider.GetComponent<Thing>().TriggerMethod?.Invoke();
        }

    }

    public bool CheckPlayer()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, (player.position - transform.position).normalized, distance, (1 << 10) | (1 << 8) | (1 << 9) | (1 << 12));
        RaycastHit2D hitNear;
        if (hits.Length >= 2)
        {
            hitNear = hits[1];
            if (hitNear.collider.tag == "player") return true;
            else return false;
        }
        else return false;
    }

    private void OnDrawGizmos()
    {
        if (player != null) Gizmos.DrawLine(transform.position, (player.position - transform.position).normalized * distance + transform.position);
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
