using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using UnityEngine.Events;

public class Enemy : MonoBehaviour{

    // Use this for initialization
    public bool canBeDamagedByKunaiDash = true;
    public GameObject target;
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
    protected Vector3 originalScale;
    private SpriteRenderer m_spRender;

    private UnityAction<int> m_takeDamageAct;

    bool justSawPlayer;
    public float sightDistance;

    protected GameObject exclamation;
    public delegate void LoseHPDelegate(int lossHP);
    public event LoseHPDelegate OnLoseHP;
    protected GameObject hpText;
    AudioClip hitClip;

    private void Awake()
    {

    }
    protected void Start () {
        if (target == null) target = PlayerControl1.Instance.gameObject;
        hitClip = Resources.Load<AudioClip>("Sounds/Toy_PopGun_Shot");
        originalScale = transform.localScale;
        exclamation = Instantiate(Resources.Load<GameObject>("exclamation"), Vector3.zero, Quaternion.identity, transform);
        exclamation.transform.localPosition = new Vector3(-40f, 40f, 0f);
        exclamation.SetActive(false);
        hpText = Instantiate(Resources.Load<GameObject>("HPCanvas"), Vector3.zero, Quaternion.identity, transform);
        hpText.transform.localPosition = new Vector3(0f, 50f, 0f);
        hpText.GetComponent<HPText>().enemy = this;
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
        m_spRender = GetComponent<SpriteRenderer>();
        if(m_spRender != null)
        {
            originalColor = m_spRender.color;
        }
        else
        {
            m_spRender = GetComponentInChildren<SpriteRenderer>();
            if(m_spRender != null)
            {
                originalColor = m_spRender.color;
            }
            else
            {
                Debug.Assert(false);
            }
        }
        
        //player = PlayerControl1.Instance.transform;
    }
	
	// Update is called once per frame
	void Update () {
		if (thing.upperY < -16000f)
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

    public void RegisteTakeDamage(UnityAction<int> _act)
    {
        m_takeDamageAct += _act;
    }
    public void RemoveTakeDamage(UnityAction<int> _act)
    {
        m_takeDamageAct -= _act;
    }

    public void TakeDamage(int damage)
	{
		health -= damage;
        OnLoseHP?.Invoke(damage);
        if (m_takeDamageAct != null)
        {
            m_takeDamageAct.Invoke(damage);
        }
        if (health <= 0)
			thing.Die ();
        else
        {
            if (GetComponent<AudioSource>() != null)
                GetComponent<AudioSource>().PlayOneShot(hitClip);
        }
        StartCoroutine(OnHit());
	}

    public void EnemyDie()
    {
        StartCoroutine(SelfDestroy());
    }
    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        //Destroy(gameObject);
    }

    IEnumerator OnHit()
    {
        Color hitColor = new Color(1f, 1f, 0f);
        if(CameraShaker.Instance != null)
        {
            CameraShaker.Instance.ShakeOnce(35f, 4f, 0.1f, 0.1f);
        }

        Vector3 scale = transform.localScale;
        transform.localScale = scale * 0.9f;
        yield return new WaitForSeconds(0.05f);
        transform.localScale = scale * 0.78f;
        yield return new WaitForSeconds(0.05f);
        transform.localScale = scale * 0.9f;
        yield return new WaitForSeconds(0.05f);
        m_spRender.color = hitColor;
        transform.localScale = originalScale;
        m_spRender.color = originalColor;
    }

    public bool CheckPlayerInSight()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, (PlayerControl1.Instance.transform.position - transform.position).normalized, sightDistance, (1 << 10) | (1 << 8) | (1 << 9));
        RaycastHit2D hitNear;
        if (hits.Length >= 2)
        {
            hitNear = hits[1];
            if (hitNear.collider.tag == "player")
            {
                justSawPlayer = true;
                return true;
            }

            else return false;
        }
        else return false;
    }
}
