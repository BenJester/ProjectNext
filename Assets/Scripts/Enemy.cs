using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour{

	// Use this for initialization
	public Thing thing;
	public float dropKillSpeed;
	public Goal goal;
	public int maxHealth = 1;
	public int health;
    public bool canShuaisi = true;

	protected void Start () {
		maxHealth = 1;
		health = maxHealth;
		thing = GetComponent<Thing> ();
		goal = GameObject.FindGameObjectWithTag ("goal").GetComponent<Goal>();
		goal.enemyCount += 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (thing.upperY < -600f)
        {
            thing.Die();
        }
	}

	void OnCollisionEnter2D (Collision2D col) {


		if (thing.prevVelocity.y < -dropKillSpeed && canShuaisi && col.transform.position.y < transform.position.y) {
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
