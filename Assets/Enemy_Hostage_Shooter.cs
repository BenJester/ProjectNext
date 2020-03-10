using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Hostage_Shooter : Enemy
{
    // Start is called before the first frame update
    public bool canShoot = true;
    public float distance;
    public GameObject bullet;
    private Vector2 direction;
    public float bulletSpeed;
    public float shootInterval = 1f;
    public float animationPreload = 0.1f;
    public float bulletInstanceDistance = 50f;
    private Animator animator;
    public Transform target;
    public bool isInSight = false;
    public LineRenderer lr;
    public GameObject areaIndicator;

    private void Awake()
    {

        animator = GetComponent<Animator>();
        //player = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();
        lr = GetComponent<LineRenderer>();
    }
    void Start()
    {

        base.Start();
        StartCoroutine(HandleShoot());
        GameObject area = Instantiate(areaIndicator, transform.position, Quaternion.identity);
        area.transform.parent = null;
        area.GetComponent<SpriteRenderer>().size = new Vector2(distance * 2, distance * 2);
        area.transform.parent = transform;
        //transform.rotation = Quaternion.Euler (0, 0, AngleBetween (direction, Vector2.left));
    }

    // Update is called once per frame
    void Update()
    {

        if (thing.dead)
        {
            lr.enabled = false;
            return;
        }
        direction = (target.position - transform.position).normalized;
        //在视线中
        isInSight = CheckPlayerInSight();

        //画线；
        if (isInSight)
        {
            lr.enabled = true;
            Debug.DrawLine(transform.position, transform.position + (Vector3)direction * distance, Color.red, 0.1f);
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, (target.position - transform.position).normalized * distance + transform.position);
        }
        else lr.enabled = false;

    }

    IEnumerator HandleShoot()
    {
       

        while (canShoot)
        {
            if (thing.dead) yield return null;
            if (isInSight && !thing.dead)
            {

                animator.CrossFade("Enemy_Shooter_Shot", 0.001f);
                exclamation.SetActive(true);
                yield return new WaitForSeconds(animationPreload);
                Shoot();
                exclamation.SetActive(false);
                yield return new WaitForSecondsRealtime((shootInterval - animationPreload) / Time.timeScale);
            }
            else
            {
                yield return null;
            }
        }

    }

    public void Shoot()
    {
        if (thing.dead)
            return;
        GameObject newBullet = Instantiate(bullet, transform.position + bulletInstanceDistance * (Vector3)direction, Quaternion.identity);
        Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();
        bulletBody.velocity = direction * bulletSpeed;

    }

    public void PowerShoot()
    {
        if (thing.dead)
            return;
        GameObject newBullet = Instantiate(bullet, transform.position + bulletInstanceDistance * (Vector3)direction, Quaternion.identity);
        Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();
        bulletBody.velocity = direction * bulletSpeed;
        newBullet.transform.localScale *= 1.5f;


        //GetComponent<Thing>().SetShield(true);
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

    public bool CheckTargetInSight()
    {


       

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, (target.position - transform.position).normalized, distance, (1 << 10) | (1 << 8) | (1 << 9));
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
        //if (target != null) Gizmos.DrawLine(transform.position, (target.position - transform.position).normalized * distance + transform.position);
    }

}
