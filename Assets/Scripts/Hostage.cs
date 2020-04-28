using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hostage : MonoBehaviour {


    //public bool hasExitArea=false;
	Thing thing;
	public float dropKillSpeed;
	Goal goal;
    public float speed;
    public float jumpSpeed;
    Transform player;
    Rigidbody2D rb;
    public LayerMask checkLayer;
    public LayerMask scanLayer;
    public LayerMask playerLayer;
    AudioSource source;
    public bool useAmmo;
    public int ammo;
    public Text ammoText;
    public AudioClip shootSound;

    public bool useLevel;
    public int level;

    void Start () {
		thing = GetComponent<Thing> ();
		goal = GameObject.FindGameObjectWithTag ("goal").GetComponent<Goal>();
		goal.hostageList.Add (GetComponent<Thing> ());
        rb = GetComponent<Rigidbody2D>();
        player = PlayerControl1.Instance.transform;
        StartCoroutine(HandleShoot());
        source = GetComponent<AudioSource>();
	}

    public float checkRange;

    public Enemy NearestEnemyInSight()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, checkRange, checkLayer);
        float minDistance = Mathf.Infinity;
        Enemy res = null;

        foreach (Collider2D col in cols)
        {
            if (col.GetComponent<Enemy>() != null && !col.GetComponent<Thing>().dead && col.gameObject != gameObject)
            {
                RaycastHit2D hit0 = Physics2D.Raycast(transform.position + (col.transform.position - transform.position).normalized * 50f * transform.localScale.x, (col.transform.position - transform.position).normalized, checkRange, scanLayer);
                RaycastHit2D hitPlayer = Physics2D.Raycast(transform.position + (col.transform.position - transform.position).normalized * 50f * transform.localScale.x, (col.transform.position - transform.position).normalized, checkRange, playerLayer);
                if (hitPlayer) return null;
                if (hit0.collider.gameObject == col.gameObject)
                {
                    float distance = Vector3.Distance(transform.position, col.transform.position);
                    if (distance < minDistance)
                    {
                        res = col.GetComponent<Enemy>();
                        minDistance = distance;
                    }
                }
            }
        }
        return res;
    }
    public GameObject bullet;
    public float bulletSpeed;
    public float bulletInstanceDistance;
    public float shootInterval;
    IEnumerator HandleShoot()
    {
        if (!shoot) yield break;
        while (true)
        {
            yield return new WaitForSeconds(shootInterval);
            Enemy target = NearestEnemyInSight();
            if (target != null && (!useAmmo || ammo >= 1))
            {
                Vector3 direction = (target.transform.position - transform.position).normalized;
                GameObject newBullet = Instantiate(bullet, transform.position + bulletInstanceDistance * (Vector3)direction, Quaternion.identity);
                Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();
                bulletBody.velocity = direction * bulletSpeed;
                ammo -= 1;
                source.PlayOneShot(shootSound);
            }
        }
        
    }
    public bool shoot;
	// Update is called once per frame
	void Update () {
		if (thing.upperY<-600f)
		{
			thing.Die();
		}
        if (useAmmo)
        {
            ammoText.text = ammo.ToString();
        }
	}

    bool detectObstacle()
    {

        return false;
    }

    void Jump()
    {
        if (thing.touchingFloor())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (speed != 0f && thing.touchingFloor())
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        if (thing.touchingWallRight())
            Jump();
    }
    void OnCollisionEnter2D (Collision2D col) {

		if (thing.prevVelocity.y < -dropKillSpeed && col.transform.position.y < transform.position.y) {
			thing.collider.enabled = false;
			thing.Die ();
		}
	}
}
