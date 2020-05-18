using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Mech_TimingLaser : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isActive = true;
    public float distance;
    public GameObject areaIndicator;
    Vector2 direction;
    Transform player;
    public Text countDownText;
    float distanceBetweenTarget;

    public GameObject smoke;

    //TODO:还没有加入动画
    public PlayerState state;
    //public Text countDownText;
    public float aimTime = 0.5f;
    float shootTime = 0f;
    float countDown;
    LineRenderer lr;
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
    int[] stateActiveTimeCounts;
    void Start()
    {
        
        lr = GetComponent<LineRenderer>();

        player = PlayerControl1.Instance.GetComponent<Transform>();
        lr.startWidth = 5;
        lr.endWidth = 5;

        GameObject area = Instantiate(areaIndicator, transform.position, Quaternion.identity);
        area.transform.parent = null;
        area.GetComponent<SpriteRenderer>().size = new Vector2(distance * 2, distance * 2);
        area.transform.parent = transform;
    }

    // Update is called once per frame
    void Update()
    {

        if (!isActive) {
            return;
        
        
        }
        StateUpdate();

        switch (state)
        {
            case PlayerState.idle:
                if (stateActiveFrames[(int)PlayerState.idle] == 0)
                {
                    countDown = aimTime;
                    countDownText.text = countDown.ToString("F1");
                    lr.enabled = false;
                }


                break;
            case PlayerState.aim:
                if (stateActiveFrames[(int)PlayerState.aim] == 0)
                {
                    lr.startWidth = 5;
                    lr.endWidth = 5;
                    lr.enabled = true;
                }


                countDown = aimTime - stateActiveFrames[(int)PlayerState.aim];
                countDownText.text = countDown.ToString(("F1"));
                lr.startWidth = stateActiveFrames[(int)PlayerState.aim] * 5;
                lr.endWidth = stateActiveFrames[(int)PlayerState.aim] * 5;

                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, (Vector3)direction * distance + transform.position);
                lr.startColor = Color.yellow;
                lr.endColor = Color.yellow;

                direction = (player.position - transform.position).normalized;
                break;

            case PlayerState.shoot:
                if(stateActiveFrames[(int)PlayerState.shoot] == 0){
                    LaserShoot();
                    lr.startWidth = 15;
                    lr.endWidth = 15;
                    lr.startColor = Color.red;
                    lr.endColor = Color.red;
                }
               
                break;

            case PlayerState.dead:
                return;

        }

        TimeUpdate();

        transform.localRotation = Quaternion.Euler(0, 0, -AngleBetween(Vector2.up, direction));
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
             if (CheckPlayer()) state = PlayerState.aim;
        }
        else if (state == PlayerState.aim)
        {
            if (stateActiveFrames[(int)PlayerState.aim] > aimTime)
            {
                state = PlayerState.shoot;
            }
            //else if (!CheckPlayer())
            //{
            //    state = PlayerState.idle;
            //}
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
        direction = (player.position - transform.position).normalized;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, distance, (1 << 8) | (1 << 9) | (1 << 22)| (1 << 12) | (1 << 18));
        RaycastHit2D hitNear;
        if (hits.Length >= 1)
        {
            hitNear = hits[0];
            distanceBetweenTarget = Vector2.Distance(transform.position, hitNear.point);
            lr.SetPosition(1, (Vector3)direction * distanceBetweenTarget + transform.position);
            GameObject part = Instantiate(smoke, hitNear.point, Quaternion.identity);
            Destroy(part, 1f);

            if (hitNear.collider.tag == "player") hitNear.collider.GetComponent<PlayerControl>().Die();
            if (hitNear.collider.tag == "thing") hitNear.collider.GetComponent<Thing>().TriggerMethod?.Invoke();
            if (hitNear.collider.tag == "thing" && hitNear.collider.GetComponent<Thing>().type == Ben.Type.enemy)
            {
                hitNear.collider.GetComponent<Enemy>().TakeDamage(4);
            }
        }
    }

    public bool CheckPlayer()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, (player.position - transform.position).normalized, distance, (1 << 8) | (1 << 18) | (1 << 9) | (1 << 12)|(1 << 22));
        RaycastHit2D hitNear;
        if (hits.Length >= 1)
        {
            hitNear = hits[0];
            if (hitNear.collider.tag == "player") return true;
            else return false;
        }
        else return false;
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

    public void CloseLaser() {
        isActive = false;
        GetComponent<SpriteRenderer>().color = Color.black;
    
    }
}
