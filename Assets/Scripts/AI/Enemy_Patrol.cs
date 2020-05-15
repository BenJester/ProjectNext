using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Patrol : Enemy
{

    //public bool faceRight;
    public float turnInterval;
    public float turnTimer;
    public float turnWarningDur;
    public float shootInterval;
    public float shootNum;
    public float shootPreload;
    public float bulletSpeed;
    public GameObject bullet;
    public float bulletInstanceDistance;
    public SpriteRenderer sr;
    public SpriteRenderer colSr;

    public Enemy_Patrol_Collider collider;
    //Vector3 originalScale;

    void Start()
    {
        base.Start();
        originalScale = transform.localScale;
        StartCoroutine(HandleTurn());
    }

    public void Detect(GameObject pos)
    {
        StartCoroutine(HandleShoot(pos));
        turnTimer = 0f;
    }

    public void Shoot(GameObject pos)
    {
        Vector2 direction = ((Vector2)pos.transform.position - (Vector2)transform.position).normalized;
        if (thing.dead)
            return;
        GameObject newBullet = Instantiate(bullet, transform.position + bulletInstanceDistance * (Vector3)direction, Quaternion.identity);
        Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();
        bulletBody.velocity = direction * bulletSpeed;
    }

    void Update()
    {
        
    }
    IEnumerator HandleTurn()
    {
        while (true)
        {
            turnTimer += Time.deltaTime;
            if (turnTimer > turnInterval)
            {
                exclamation.SetActive(true);
                yield return new WaitForSeconds(turnWarningDur);
                exclamation.SetActive(false);

                Turn();

                turnTimer = 0f;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    void Turn()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    IEnumerator HandleShoot(GameObject pos)
    {
        int count = 0;
        exclamation.SetActive(true);
        yield return new WaitForSeconds(shootPreload);
        while (count < shootNum)
        {
            count += 1;
            Shoot(pos);
            yield return new WaitForSeconds(shootInterval);
        }
        exclamation.SetActive(false);
    }
}
