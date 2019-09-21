using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour{

	// Use this for initialization
	[HideInInspector]
	public Thing thing;
	public float dropKillSpeed=50f;
	[HideInInspector]
	public Goal goal;
	public int maxHealth = 1;
	public int health=1;
    public bool canShuaisi = false;
    BoxCollider2D box;
    bool grounded;
    Vector2 groundCheckTopLeft;
    Vector2 groundCheckBottomRight;
    float groundCheckBoxIndent = 2f;
    public float groundCheckBoxHeight = 60f;
    public LayerMask floorLayer = 8;

    protected void Start () {
		maxHealth = 1;
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
    }
	
	// Update is called once per frame
	void Update () {
		if (thing.upperY < -600f)
        {
            thing.Die();
        }
	}

	void OnCollisionEnter2D (Collision2D col) {

        grounded = Physics2D.OverlapArea
    (
        (Vector2)transform.position + groundCheckTopLeft,
        (Vector2)transform.position + groundCheckBottomRight,
        floorLayer
    );

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
	}
}
