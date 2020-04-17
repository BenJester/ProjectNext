using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Ben;
namespace Ben
{
    public enum Type
    {
        player = 1,
        enemy = 2,
        box = 3,
        hostage = 4,
        bomb = 5,
        invincible = 6
    }
}
public class Thing : MonoBehaviour {
    public bool beingThrown;
    public SpriteRenderer sr;
    public GameObject SpawnObjOnDie;
    public GameObject dieParticle;
    public UnityEvent TriggerMethod;
    public UnityEvent swapTriggerMethod;
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
    public bool isHeld;
    public bool isKey;
    public GameObject shield;
	GameObject player;
	[Header("死亡动画，需要有HeadBodySeparation脚本")]
	public bool isDivedeDead=false;
	public float force = 25000f;

    public bool faceRight;
    public bool swapping;

    public float MomentumMass;

    private UnityAction m_swapAction;

    private UnityAction m_acDestroy;

    private Quaternion m_qtOriginalQuat;
    private bool m_bThingSwap;
    public bool IsSwapRotationByVelocity;
	
	
	[HideInInspector]

	public float distanceToCursor = Mathf.Infinity;
    public float distanceToPlayer = Mathf.Infinity;
    
    private SpriteRenderer m_spRender;

    public delegate void OnDieDelegate();
    public event OnDieDelegate OnDie;

    float wallCheckBoxWidth = 10f;
    float wallCheckBoxIndent = 2f;

    Vector2 wallCheckTopLeft;
    Vector2 wallCheckBottomRight;
    Vector2 leftWallCheckTopLeft;
    Vector2 leftWallCheckBottomRight;

    Vector2 upWallCheckTopLeft;
    Vector2 upWallCheckBottomRight;
    Vector2 floorCheckTopLeft;
    Vector2 floorCheckBottomRight;
    public LayerMask TouchLayer;
    public float gravity;
    public bool isTouchingGround;
    [Header("是否是标准重力？")]
    public bool isStandardGravity = true;
    #region wallTouchChecks
    void InitWallChecks()
    {
        wallCheckTopLeft = new Vector2
                         (
                            (collider.size.x / 2f - wallCheckBoxWidth / 2f),
                            (collider.size.y / 2f - wallCheckBoxIndent)
                         );
        wallCheckBottomRight = new Vector2
                                 (
                                    collider.size.x / 2f + wallCheckBoxWidth / 2f,
                                    -(collider.size.y / 2f - wallCheckBoxIndent)
                                 );
        leftWallCheckTopLeft = new Vector2
                         (
                            -(collider.size.x / 2f + wallCheckBoxWidth / 2f),
                            (collider.size.y / 2f - wallCheckBoxIndent)
                         );
        leftWallCheckBottomRight = new Vector2
                                 (
                                    -(collider.size.x / 2f - wallCheckBoxWidth / 2f),
                                    -(collider.size.y / 2f - wallCheckBoxIndent)
                                 );
        upWallCheckTopLeft = new Vector2
                         (
                            -(collider.size.x / 2f - wallCheckBoxIndent),
                            collider.size.y / 2f + wallCheckBoxWidth / 2f
                         );
        upWallCheckBottomRight = new Vector2
                                 (
                                    collider.size.x / 2f - wallCheckBoxIndent,
                                    collider.size.y / 2f - wallCheckBoxWidth / 2f
                                 );
        floorCheckTopLeft = new Vector2
                         (
                            -(collider.size.x / 2f - wallCheckBoxIndent),
                            -(collider.size.y / 2f - wallCheckBoxWidth / 2f)
                         );
        floorCheckBottomRight = new Vector2
                                 (
                                    collider.size.x / 2f - wallCheckBoxIndent,
                                    -(collider.size.y / 2f + wallCheckBoxIndent)
                                 );
    }
    public bool touchingWallRight()
    {
        var col = Physics2D.OverlapAreaAll
                (
                    (Vector2)transform.position + wallCheckTopLeft,
                    (Vector2)transform.position + wallCheckBottomRight,
                    TouchLayer
                );
        return col.Length > 1;
    }
    public bool touchingWallLeft()
    {
        return Physics2D.OverlapArea
                (
                    (Vector2)transform.position + leftWallCheckTopLeft,
                    (Vector2)transform.position + leftWallCheckBottomRight,
                    TouchLayer
                );
    }
    public bool touchingWallUp()
    {
        return Physics2D.OverlapArea
                (
                    (Vector2)transform.position + upWallCheckTopLeft,
                    (Vector2)transform.position + upWallCheckBottomRight,
                    TouchLayer
                );
    }
    public bool touchingFloor()
    {
        var col = Physics2D.OverlapAreaAll
                (
                    (Vector2)transform.position + floorCheckTopLeft,
                    (Vector2)transform.position + floorCheckBottomRight,
                    TouchLayer
                );
        return col.Length > 1;
    }
    #endregion

    public IEnumerator CancelBeingThrown(float delay)
    {
        beingThrown = true;
        yield return new WaitForSeconds(delay);
        beingThrown = false;
    }
    public virtual void Start () {
        slipperyDur = 0.2f;
        slipperyTime = 1f;
        m_spRender = GetComponent<SpriteRenderer>();
        if(m_spRender == null)
        {
            m_spRender = GetComponentInChildren<SpriteRenderer>();
        }
        m_qtOriginalQuat = transform.rotation;
        if ( MomentumMass == 0.0f )
        {
            MomentumMass = 10;
        }
		player = GameObject.FindWithTag ("player");
		playerControl = player.GetComponent<PlayerControl1> ();
		originalScale = transform.localScale;
		collider = GetComponent<BoxCollider2D> ();

        
        body = GetComponent<Rigidbody2D> ();
        
        GameObject goalObject = GameObject.FindWithTag ("goal");
		if (goalObject != null)
			goal = goalObject.GetComponent<Goal>();

        if (hasShield && GetComponentInChildren<Shield>() == null)
            Instantiate(Resources.Load<GameObject>("shield"), transform.position, Quaternion.identity, transform);
        if (isKey)
            Instantiate(Resources.Load<GameObject>("key"), new Vector3(transform.position.x - 70f, GetUpperY() + 60f, transform.position.z) , Quaternion.identity, transform);
        //HandleRewind();
        if (isHeld)
        {
            var obj = Instantiate(Resources.Load<GameObject>("rope"), new Vector3(transform.position.x, GetUpperY() + 60f, transform.position.z), Quaternion.identity, transform);
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            obj.GetComponent<BombHolder>().thing = this;
        }
            
        
        if (type != Type.player) 
		{
			playerControl.thingList.Add (this);
		}
        if (body.gravityScale != 0f && isStandardGravity)
            body.gravityScale = 165f;
        if (PlayerControl1.Instance.GetComponent<InvertGravity>() != null)
            body.gravityScale = !PlayerControl1.Instance.GetComponent<InvertGravity>().even ? Mathf.Abs(body.gravityScale) : -Mathf.Abs(body.gravityScale);
        gravity = body.gravityScale;
        InitWallChecks();
    }
    public Quaternion GetOriginalQuat()
    {
        return m_qtOriginalQuat;
    }
    public void RegisteSwap(UnityAction ac)
    {
        m_swapAction += ac;
    }
    public void UnregisteSwap(UnityAction ac)
    {
        m_swapAction -= ac;
    }
    public void ThingSwap()
    {
        if(m_swapAction != null)
        {
            m_swapAction.Invoke();
        }
        SetThingSwap(true);
    }
    public void SetThingSwap(bool bSwap)
    {
        m_bThingSwap = bSwap;
    }
    public bool IsThingSwap()
    {
        return m_bThingSwap;
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
    public float GetLowerY()
    {
        return lowerY = transform.position.y - collider.bounds.size.y / 2f;
    }
    public float GetUpperY()
    {
        return upperY = transform.position.y + collider.bounds.size.y / 2f;
    }

    public float GetLeftX()
    {
        return leftX = transform.position.x - collider.bounds.size.x / 2f;
    }
    public float GetRightX()
    {
        return rightX = transform.position.x + collider.bounds.size.x / 2f;
    }
    public virtual void Update () {
		lowerY = transform.position.y - collider.bounds.size.y / 2f;
		upperY = transform.position.y + collider.bounds.size.y / 2f;
		leftX = transform.position.x - collider.bounds.size.x / 2f;
		rightX = transform.position.x + collider.bounds.size.x / 2f;

		if (transform.position.y < -8000f)
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
        isTouchingGround = body.gravityScale > 0f && Mathf.Abs(body.velocity.y) < 1f;

    }

	public void Die() {
        //		if (type == Type.player && playerControl.isWorld) {
        //			player.transform.position = CheckPointTotalManager.instance.savedPos;
        //			return;
        //		}
        if (type == Type.player)
        {
            PlayerControl1.Instance.Die();
            playerControl.hp = 0;
        }

        if (SpawnObjOnDie != null)
        {
            GameObject obj = Instantiate(SpawnObjOnDie, transform.position, Quaternion.identity);
            if (obj.CompareTag("thing"))
                obj.GetComponent<Thing>().TriggerMethod?.Invoke();
        }

        if (dead)
			return;
		dead = true;
        OnDie?.Invoke();
        if (type == Type.enemy)
        {
            goal.enemyCount -= 1;
            hasShield = false;
            gameObject.GetComponent<Enemy>().EnemyDie();
        }
        if (dieParticle!=null)
        {
            Instantiate(dieParticle, transform.position, Quaternion.identity);
        }
		if(isDivedeDead){
			GetComponent<HeadBodySeparation>().Dead(force);
			//print("die");
		}

        if (PlayerControl1.Instance.swap.overheadRB != null && PlayerControl1.Instance.swap.overheadRB.gameObject == gameObject)
            PlayerControl1.Instance.swap.DropOverhead();
		StartCoroutine (ScaleDown(0.2f));
	}

	IEnumerator ScaleDown(float duration) {
		gameObject.GetComponent<BoxCollider2D>().enabled = false;
        m_spRender.enabled = false;
		gameObject.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Static;

		while (transform.localScale.x >= 0.02) {
			float perc = Time.deltaTime / duration;
			transform.localScale -= perc * originalScale;
			yield return new WaitForEndOfFrame ();
		}
	}

	IEnumerator ScaleUp(float duration) {
		gameObject.GetComponent<BoxCollider2D>().enabled = true;
        m_spRender.enabled = true;
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

    private void OnDestroy()
    {
        if(m_acDestroy != null)
        {
            m_acDestroy.Invoke();
        }
    }

    public void RegisteDestroyNotify( UnityAction ac)
    {
        m_acDestroy += ac;
    }

    public void SetShield(bool has){
        hasShield = has;
        if (hasShield && GetComponentInChildren<Shield>() == null)
            Instantiate(Resources.Load<GameObject>("shield"), transform.position, Quaternion.identity, transform);
    }

    public void Fall()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        TriggerMethod?.Invoke();
    }
    public Vector2 thrownDir;
    public bool slippery;
    public float slipperyTime;
    public float slipperyDur;
    //public float 
    public IEnumerator SetSlippery()
    {
        slippery = true;
        yield return new WaitForSeconds(slipperyTime);
        slippery = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (slippery && !touchingFloor())
        {
            StartCoroutine(SlipperyThrow());
        }
    }
    IEnumerator SlipperyThrow()
    {
        float timer = 0f;
        slippery = false;
        while (timer < slipperyDur)
        {
            body.velocity = thrownDir * playerControl.swap.swapSpeed * 0.35f;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
            
        }
    }
}
