using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ninja : Enemy
{
    public enum State
    {
        idle = 0,
        walk = 1,
        shoot = 2,
        swap = 3,
        dash = 4,
        swapBullet = 5,
    }

    public State state;
    public bool busy;

    Transform player;
    PlayerControl1 playerControl;
    Rigidbody2D body;
    BoxCollider2D playerBox;

    public float sightDistance;

    public GameObject bullet;
    public float bulletInstanceDistance;
    public float shootDelay;
    public int shootCount;
    public float shootInteval;
    public float bulletSpeed;

    public float dashDelay;
    public int dashCount;
    public float dashInteval;
    public float dashSpeed;
    public float dashDuration;
    public float hitboxWidth;

    public float walkSpeed;
    public float walkDuration;

    public float idleDuration;

    public float dashThreshold;
    public float shootThreshold;

    public float bombDelay;
    public float bombSpeed;
    public GameObject bomb;

    public float attackInteval;
    public bool justAttacked;

    void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();
        playerControl = player.GetComponent<PlayerControl1>();

        body = GetComponent<Rigidbody2D>();

        playerBox = player.GetComponent<BoxCollider2D>();
    }

    public bool CheckPlayerInSight()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, (player.position - transform.position).normalized, sightDistance, (1 << 10) | (1 << 8) | (1 << 9));
        RaycastHit2D hitNear;
        if (hits.Length >= 2)
        {
            hitNear = hits[1];
            if (hitNear.collider.tag == "player") return true;
            else return false;
        }
        else return false;
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (!CheckPlayerInSight() || thing.dead) return;

        if (!justAttacked)
        {
            StartCoroutine(ThrowBomb());
            if (distance < dashThreshold)
            {
                StartCoroutine(Dash());
            }
            else
            {
                StartCoroutine(Shoot());
            }
        }
        else
        {
            
            if (distance < dashThreshold)
            {
                StartCoroutine(Idle());
            }
            else
            {
                StartCoroutine(Walk());
            }
        }
    }

    IEnumerator StartAttackTimer()
    {
        justAttacked = true;
        float timer = 0f;
        while (timer < attackInteval)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        justAttacked = false;
    }

    IEnumerator Idle()
    {
        if (busy) yield break;
        busy = true;

        yield return new WaitForSeconds(idleDuration);
        busy = false;
    }

    IEnumerator Walk()
    {
        if (busy) yield break;
        busy = true;

        float timer = 0f;
        body.velocity = new Vector2(player.position.x < transform.position.x ? -walkSpeed : walkSpeed, body.velocity.y);
        while (timer < walkDuration)
        {
            if (!grounded) break;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        busy = false;
    }

    IEnumerator Shoot()
    {
        if (busy) yield break;
        busy = true;
        yield return new WaitForSeconds(shootDelay);

        for (int i = 0; i < shootCount; i ++)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            GameObject newBullet = Instantiate(bullet, transform.position + bulletInstanceDistance * (Vector3)direction, Quaternion.identity);
            Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();
            bulletBody.velocity = direction * bulletSpeed;
            yield return new WaitForSeconds(shootInteval);
        }
        busy = false;
        StartCoroutine(StartAttackTimer());
    }

    IEnumerator ThrowBomb()
    {
        if (busy) yield break;
        busy = true;
        Vector3 direction = (Vector3.up * 2f + (player.position - transform.position).normalized).normalized;
        GameObject newBullet = Instantiate(bomb, transform.position + bulletInstanceDistance * (Vector3)direction, Quaternion.identity);
        Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();
        bulletBody.velocity = direction * bombSpeed;
        busy = false;
        StartCoroutine(StartAttackTimer());
    }

    IEnumerator Dash()
    {
        if (busy) yield break;
        busy = true;
        yield return new WaitForSeconds(dashDelay);

        for (int i = 0; i < dashCount; i++)
        {
            
            Vector3 direction = (player.position - transform.position).normalized;
            float timer = 0f;
            while (timer < dashInteval)
            {
                body.velocity = new Vector2(0, 0);
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            timer = 0f;
            while (timer < dashDuration)
            {
                Physics2D.IgnoreCollision(box, playerBox, true);
                body.velocity = direction * dashSpeed;
                Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position + direction * (box.size.x + hitboxWidth),
                                 new Vector2(hitboxWidth * 2, box.size.y),
                                 Vector2.SignedAngle(transform.position, player.position));

                foreach (Collider2D col in cols)
                {
                    if (col.CompareTag("player"))
                        playerControl.Die();
                }
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
                Physics2D.IgnoreCollision(box, playerBox, false);
            }
            timer = 0f;
            body.velocity = Vector2.zero;
        }
        busy = false;
        StartCoroutine(StartAttackTimer());
    }
}
