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
    public GameObject SpawnObjOnDie;
    public GameObject dieParticle;
    public UnityEvent TriggerMethod;
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
    public GameObject shield;
	GameObject player;
	[Header("死亡动画，需要有HeadBodySeparation脚本")]
	public bool isDivedeDead=false;
	public float force = 25000f;

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


    public virtual void Start () {
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


        //HandleRewind();

        if (type != Type.player) 
		{
			playerControl.thingList.Add (this);
		}
        if (body.gravityScale != 0f)
            body.gravityScale = 165f;
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
}
