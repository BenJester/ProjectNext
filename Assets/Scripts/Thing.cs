using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type {
	player = 1,
	enemy = 2,
	box = 3,
	hostage = 4
}

public class Thing : MonoBehaviour {

	
    public GameObject dieParticle;
	public Type type;
	public float lowerY;
	public float upperY;
	public float leftX;
	public float rightX;
	public BoxCollider2D collider;
	public Rigidbody2D body;
	public Vector2 prevVelocity;
	PlayerControl1 playerControl;
	Vector3 originalScale;
	Goal goal;
	public bool dead = false;
    public bool hasShield = false;
	GameObject player;
	[Header("死亡动画，需要有HeadBodySeparation脚本")]
	public bool isDivedeDead=false;
	public float force = 25000f;
	
	
	[HideInInspector]

	public float distanceToCursor = Mathf.Infinity;
    public float distanceToPlayer = Mathf.Infinity;

	public virtual void Start () {
		player = GameObject.FindWithTag ("player");
		playerControl = player.GetComponent<PlayerControl1> ();
		originalScale = transform.localScale;
		collider = GetComponent<BoxCollider2D> ();
		body = GetComponent<Rigidbody2D> ();
		GameObject goalObject = GameObject.FindWithTag ("goal");
		if (goalObject != null)
			goal = goalObject.GetComponent<Goal>();


        //HandleRewind();

        if (type != Type.player) 
		{
			playerControl.thingList.Add (this);
		}

	}
	
    void HandleRewind()
    {
        switch (type)
        {
            case Type.box:
                Rewind.Instance.obj.Add(gameObject);
                break;
            case Type.enemy:
                Rewind.Instance.enemies.Add(gameObject);
                break;
            case Type.hostage:
                Rewind.Instance.enemies.Add(gameObject);
                break;
            default:
                break;
        }
    }

	public virtual void Update () {
		lowerY = transform.position.y - collider.size.y / 2f * transform.localScale.y;
		upperY = transform.position.y + collider.size.y / 2f * transform.localScale.y;
		leftX = transform.position.x - collider.size.x / 2f * transform.localScale.x;
		rightX = transform.position.x + collider.size.x / 2f * transform.localScale.x;

		if (transform.position.y < -3000f)
			Die ();

//		if (type != Type.player) 
//		{
//			distanceToCursor = Vector2.Distance(((Vector2) Camera.main.ScreenToWorldPoint (Input.mousePosition)), (Vector2) transform.position);
//			if (distanceToCursor < playerControl.closestDistance) 
//			{
//				playerControl.closestObjectToCursor = gameObject;
//				playerControl.closestDistance = distanceToCursor;
//			}
//				
//		}

	}

	void FixedUpdate () {
		prevVelocity = body.velocity;
	}

	public void Die() {
        //		if (type == Type.player && playerControl.isWorld) {
        //			player.transform.position = CheckPointTotalManager.instance.savedPos;
        //			return;
        //		}
        if (type == Type.player)
            playerControl.hp = 0;
		if (dead)
			return;
		dead = true;
		
		if (type == Type.enemy)
        {
            goal.enemyCount -= 1;
            hasShield = false;
        }
        if (dieParticle!=null)
        {
            Instantiate(dieParticle, transform.position, Quaternion.identity);
        }
		if(isDivedeDead){
			GetComponent<HeadBodySeparation>().Dead(force);
			//print("die");
		}


		StartCoroutine (ScaleDown(0.2f));
	}

	IEnumerator ScaleDown(float duration) {
		gameObject.GetComponent<BoxCollider2D>().enabled = false;
		gameObject.GetComponent<SpriteRenderer>().enabled = false;
		gameObject.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Static;

		while (transform.localScale.x >= 0.02) {
			float perc = Time.deltaTime / duration;
			transform.localScale -= perc * originalScale;
			yield return new WaitForEndOfFrame ();
		}
	}

	IEnumerator ScaleUp(float duration) {
		gameObject.GetComponent<BoxCollider2D>().enabled = true;
		gameObject.GetComponent<SpriteRenderer>().enabled = true;
		gameObject.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic;
		while (transform.localScale.x <= originalScale.x) {
			float perc = Time.deltaTime / duration;
			transform.localScale += perc * originalScale;
			yield return new WaitForEndOfFrame ();
		}
		transform.localScale = originalScale;

	}

	public void Revive() {
		dead = false;
		if (type == Type.enemy)
			goal.enemyCount += 1;

		StartCoroutine (ScaleUp(0.2f));
	}
}
