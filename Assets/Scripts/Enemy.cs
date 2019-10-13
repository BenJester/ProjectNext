using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Enemy : MonoBehaviour{

	// Use this for initialization
	[HideInInspector]
	public Thing thing;
	public float dropKillSpeed=50f;
	[HideInInspector]
	public Goal goal;
	public int maxHealth = 1;
	public int health = 1;
    public bool canShuaisi = false;
    protected BoxCollider2D box;
    protected bool grounded;
    protected Vector2 groundCheckTopLeft;
    protected Vector2 groundCheckBottomRight;
    protected float groundCheckBoxIndent = 2f;
    public float groundCheckBoxHeight = 60f;
    public LayerMask floorLayer = 8;
    protected Color originalColor;

    protected void Start () {
		//maxHealth = 1;
		health = maxHealth;
		thing = GetComponent<Thing> ();
		goal = GameObject.FindGameObjectWithTag ("goal").GetComponent<Goal>();
		goal.enemyCount += 1;
        box = GetComponent<BoxCollider2D>();
        groundCheckTopLeft = new Vector2
                                 (
                                    -(box.size.x / 2f - groundCheckBoxIndent),
                                    -(box.size.y / 2f - groundCheckBoxHeight / 2f)
                                 );
        groundCheckBottomRight = new Vector2
                                 (
                                    box.size.x / 2f - groundCheckBoxIndent,
                                    -(box.size.y / 2f + groundCheckBoxHeight / 2f)
                                 );
        originalColor = GetComponent<SpriteRenderer>().color;
    }
	
	// Update is called once per frame
	void Update () {
		if (thing.upperY < -1600f)
        {
            thing.Die();
        }
        grounded = Physics2D.OverlapArea
    (
        (Vector2)transform.position + groundCheckTopLeft,
        (Vector2)transform.position + groundCheckBottomRight,
        floorLayer
    );
    }

	void OnCollisionEnter2D (Collision2D col) {
        if (thing.prevVelocity.y < -dropKillSpeed && canShuaisi && grounded) {
			thing.collider.enabled = false;
			thing.Die ();
		}
	}

	public void TakeDamage(int damage)
	{
		health -= damage;
		if (health <= 0)
			thing.Die ();
        StartCoroutine(OnHit());
	}

    IEnumerator OnHit()
    {
        Color hitColor = new Color(1f, 0.15f, 0f);
        CameraShaker.Instance.ShakeOnce(35f, 4f, 0.1f, 0.1f);

        GetComponent<SpriteRenderer>().color = hitColor;
        yield return new WaitForSeconds(0.3f);
        GetComponent<SpriteRenderer>().color = originalColor;
    }
}
