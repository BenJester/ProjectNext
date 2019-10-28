using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EZCameraShake;
using UnityEngine.UI;
using UnityEngine.Events;

//using UnityEngine.Rendering.LWRP;

public class PlayerControl1 : PlayerControl {
    [Tooltip("处理角色在空中时候，平行速度迅速递减的lerp值")]
    [Range(0,1)]
    public float JumpVelocityLerp;
	public bool HasRepawnPoint = false;

    [Header("基本参数")]
    public int maxhp = 1;
    public int hp;
    bool invincible;
	public float speed;
    [Tooltip("跳跃中水平移动最大速度向上")]
    public float JumpingHorizontalUpMaxSpeed;
    [Tooltip("跳跃中水平移动施加力值向上")]
    public float JumpingHorizontalUpForce;
    [Tooltip("跳跃中水平移动最大速度向下")]
    public float JumpingHorizontalDownMaxSpeed;
    [Tooltip("跳跃中水平移动施加力值向下")]
    public float JumpingHorizontalDownForce;
    public float jumpSpeed;

    [Tooltip("空中跳跃施加力")]
    public float jumpForceAir;
    public float coyoteTime;
	public float cacheJumpTime;
	bool cachedJump;
	public float maxSpeed = 10000f;
	public Transform groundCheckPoint1;
	public Transform groundCheckPoint2;
	public Transform groundCheckPoint3;
	public Transform groundCheckPoint4;
	public Transform groundCheckPoint5;
    public PhysicsMaterial2D slipperyMat;
    public PhysicsMaterial2D roughMat;
    BoxCollider2D box;
	public float groundCheckRadius;

    public int waitTime;
    public int currWaitTime;

    public Image blood;
    public float bloodEffectDuration;

	public bool canMove;

	public Vector3 playerRespawnPoint;

	#region 符咒相关内容
	[Header ("子弹参数")]
	public GameObject bullet;

	[Space]
	[Header ("点击直接瞬间交换，不会被阻挡")]
	public bool ClickChangeDirectly;

	[Header ("激光枪射击，瞬间交换，会被阻挡")]
	public bool laserBullet = false;
    [Header("lock first, then 激光枪射击，瞬间交换，会被阻挡")]
    public bool lockLaserBullet = false;
    [Header ("带有跟踪效果")]
	public bool isHomingBullet = false;
	public float homingBulletSpeed=10f;
	public float homingBulletRotateSpeed=200f;

	[Header ("带有射击距离限制")]
	public bool hasShootDistance = true;
	//按照玩家的距离来计算
	public bool countDistanceToPlayer=true;
	public float shootDistance = 500f;

	[SerializeField]
	UnityEngine.Experimental.Rendering.LWRP.Light2D shootDistanceLight;

	

	[Space]

	[Header ("子弹速度")]
	public float minBulletSpeed;
	public float maxBulletSpeed;
	public float bulletChargeSpeed;
	public float bulletSpeed;

	[Space]

	#endregion

	[Space]
	//public LayerMask groundLayer;
	public bool isTouchingGround;
	public bool canJump;


    private bool m_bJumpingWindow;
    public bool m_bJumpRelease;

    public int startChargeFrame;
	float chargeFrame = 0;

	[Header ("瞄准表现")]

	public bool useLineRenderer = false;
	public bool useCursor = false;
	public LineRenderer lr;
	GameObject cursor;
	public GameObject cursorPrefab;
    bool locked;
	Rigidbody2D rb;

	[HideInInspector]
	public float startDeltaTime;
	[HideInInspector]
	public float targetDeltaTime;
	[HideInInspector]
	public float targetTimeScale;

	//
	private bool isGroundTemp;

	[Space]
	[Space]
	public GameObject landingParticle;

	public Vector3 originalScale;

	private Animator anim;
	public Animator legAnim;
	public SpriteRenderer legsSpriteRenderer;
	private SpriteRenderer spriteRenderer;

	private int chargeCounter = 0;

	SpriteRenderer blackSr;
	bool leftPressed;

	public GameObject pointer;
	public GameObject swapTarget;

	public List<Thing> thingList;
	public GameObject closestObjectToCursor;
	public GameObject closestObjectToPlayer;
	public float closestDistance = Mathf.Infinity;
	public float closestPlayerDistance = Mathf.Infinity;
	public float cursorSnapThreshold;
	public GameObject marker;

	public GameObject targetMarker;

	public Swap swap;
	public Dash dash;
	public bool doubleSwap;

    public LayerMask TouchLayer;
    public LayerMask BoxLayer;
    public LayerMask MovePlatformLayer;

    [Tooltip("空中跳跃施加力的时间")]
    public float JumpAddForceTime;
    private float m_fCurrentKeepJumping;
    private float m_fTotalForce;

    private float m_fHeight;

    public SpriteRenderer colShadow;
    public SpriteRenderer playerShadow;
    private bool isPlayColShadow=false;

    private LevelTest levelTest;
	public void InitSkills () {
		swap = GetComponent<Swap> ();
		dash = GetComponent<Dash> ();
	}

    public LineRenderer lockedOnObjectLine;

    private PlayerStateManager m_stateMgr;

    private UnityAction<PlayerControl1> m_playDieAction;

    void Awake () {

		originalScale = transform.localScale;
		startDeltaTime = Time.fixedDeltaTime;
		targetDeltaTime = startDeltaTime;
		targetTimeScale = 1f;
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		lr = GetComponent<LineRenderer> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		lr.enabled = false;
        box = GetComponent<BoxCollider2D>();
	}

	void Start () {
		levelTest = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelTest>();
		m_playDieAction += GlobalVariable.GetUIPlayerCtrl().PlayerDieAction;
                m_stateMgr = GetComponent<PlayerStateManager>();
        lockedOnObjectLine.startWidth = 1f;
        lockedOnObjectLine.positionCount = 2;

        blackSr = GameObject.FindWithTag ("black").GetComponent<SpriteRenderer> ();
		bulletSpeed = minBulletSpeed;

		InitSkills ();

		if (HasRepawnPoint)
			transform.position = CheckPointTotalManager.instance.GetPlayerPos ();

		//设置射程和灯光
		if (hasShootDistance) HandleShootDistanceAndLight ();
        hp = maxhp;
    }
    public void RegisteDieAction(UnityAction<PlayerControl1> _act)
    {
        m_playDieAction += _act;
    }
    private void OnDestroy()
    {
        //GlobalVariable.GetUIPlayerCtrl().UnregisteDelayRestart(_delayAction);
        if(GlobalVariable.GetUIPlayerCtrl() != null)
        {
            m_playDieAction -= GlobalVariable.GetUIPlayerCtrl().PlayerDieAction;
        }
    }

    private bool _isTouching(ref RaycastHit2D _refRaycast)
    {
        bool bRes = false;
        if (_refRaycast)
        {
            transform.SetParent(null);
            if ((BoxLayer & (1 << _refRaycast.transform.gameObject.layer)) != 0)
            {
                //如果检测出来的是boxlayer物件，判断角色与触碰物的位置，如果位置不符合条件，则不算碰触，避免在触碰物两边也会卡住的问题
                BoxCollider2D _box = _refRaycast.transform.GetComponent<BoxCollider2D>();
                float fHeight = _box.size.y / 2 * _refRaycast.transform.localScale.y;
                float posTopBox = _refRaycast.transform.position.y + fHeight;
                if (transform.position.y < posTopBox)
                {
                    bRes = false;
                }
                else
                {
                    bRes = true;
                }
            }
            else if ((MovePlatformLayer & (1 << _refRaycast.transform.gameObject.layer)) != 0)
            {
                bRes = true;
                transform.SetParent(_refRaycast.transform);
            }
            else
            {
                bRes = true;
            }
        }
        return bRes;
    }
	void Update () {

		anim.SetFloat ("SpeedY", rb.velocity.y);
        //isTouchingGround = Physics2D.Raycast (groundCheckPoint1.position, Vector3.down, 5f, (1 << 11) | (1 << 8) | (1 << 12)) || 
        //          Physics2D.Raycast (groundCheckPoint2.position, Vector3.down, 5f, (1 << 11) | (1 << 8) | (1 << 12)) || 
        //          Physics2D.Raycast (groundCheckPoint3.position, Vector3.down, 5f, (1 << 11) | (1 << 8) | (1 << 12)) || 
        //          Physics2D.Raycast (groundCheckPoint4.position, Vector3.down, 5f, (1 << 11) | (1 << 8) | (1 << 12)) || 
        //          Physics2D.Raycast (groundCheckPoint5.position, Vector3.down, 5f, (1 << 11) | (1 << 8) | (1 << 12));
        RaycastHit2D _ray1 = Physics2D.Raycast(groundCheckPoint1.position, Vector3.down, 5f, TouchLayer);
        RaycastHit2D _ray2 = Physics2D.Raycast(groundCheckPoint2.position, Vector3.down, 5f, TouchLayer);
        RaycastHit2D _ray3 = Physics2D.Raycast(groundCheckPoint3.position, Vector3.down, 5f, TouchLayer);
        RaycastHit2D _ray4 = Physics2D.Raycast(groundCheckPoint4.position, Vector3.down, 5f, TouchLayer);
        RaycastHit2D _ray5 = Physics2D.Raycast(groundCheckPoint5.position, Vector3.down, 5f, TouchLayer);
        isTouchingGround = _ray1 | _ray2 | _ray3 | _ray4 | _ray5;

        if (isTouchingGround == true)
        {
            isTouchingGround = _isTouching(ref _ray1) | _isTouching(ref _ray2) | _isTouching(ref _ray3) | _isTouching(ref _ray4) | _isTouching(ref _ray5);
        }
        // landing
        if (isTouchingGround != isGroundTemp && isTouchingGround == true && landingParticle != null)
        {
			GameObject part = Instantiate (landingParticle, transform.position - Vector3.up * 10, Quaternion.identity);
			Destroy (part, 2f);
            if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && Mathf.Abs(rb.velocity.y) <= 5f)
                rb.velocity = Vector2.zero;
            
		}

        box.sharedMaterial = isTouchingGround ? roughMat : slipperyMat;

		isGroundTemp = isTouchingGround;

		HandleRewind ();

		

		if (Input.GetKeyDown (KeyCode.R)) {
			Time.fixedDeltaTime = startDeltaTime;
			Time.timeScale = 1f;
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}

		//if (!Rewind.Instance.isReverting) {
		//	Time.timeScale = Mathf.Clamp (Time.timeScale + ((targetTimeScale >= Time.timeScale) ? 0.04f : -0.04f), 0.1f, 1f);
		//	Time.fixedDeltaTime = Mathf.Clamp (Time.fixedDeltaTime + ((targetDeltaTime >= Time.fixedDeltaTime) ? 0.04f * startDeltaTime : -0.04f * startDeltaTime), 0.1f * startDeltaTime, startDeltaTime);
		//}

		Time.timeScale = Mathf.Clamp (Time.timeScale + ((targetTimeScale >= Time.timeScale) ? 0.07f : -0.07f), 0.01f, 1f);
		Time.fixedDeltaTime = Mathf.Clamp (Time.fixedDeltaTime + ((targetDeltaTime >= Time.fixedDeltaTime) ? 0.07f * startDeltaTime : -0.07f * startDeltaTime), 0.01f * startDeltaTime, startDeltaTime);

		if (!active)
			return;


		//左右移动
		float h = (Input.GetKey (KeyCode.D) ? 1 : 0) + (Input.GetKey (KeyCode.A) ? -1 : 0);
        //if (!canMove) h = 0f;
		if (Mathf.Abs (h) > 0) {
            m_bJumpRelease = false;

            anim.SetBool ("Moving", true);
			legAnim.SetBool ("Moving", true);
			if (h > 0) legsSpriteRenderer.flipX = true;
			else legsSpriteRenderer.flipX = false;
		} else
        {
            m_bJumpRelease = false;
            anim.SetBool ("Moving", false);
			legAnim.SetBool ("Moving", false);
		}

        //if (Mathf.Abs(rb.velocity.x) <= speed)
        //{
        //    rb.velocity = new Vector2(h * speed, Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed));
        //}
        //else
        //{
        //    rb.velocity = new Vector2(h * rb.velocity.x < 0 ? rb.velocity.x + 6f * h : rb.velocity.x, Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed));
        //}

        if (canJump == false)
        {
            if (h > 0)
            {
                if(rb.velocity.y > 0)
                {
                    if (rb.velocity.x < JumpingHorizontalUpMaxSpeed)
                    {
                        rb.AddForce(Vector2.right * JumpingHorizontalUpForce);
                    }
                }
                else
                {
                    if (rb.velocity.x < JumpingHorizontalDownMaxSpeed)
                    {
                        rb.AddForce(Vector2.right * JumpingHorizontalDownForce);
                    }
                }
            }
            else if (h < 0)
            {
                if (rb.velocity.y > 0)
                {
                    if (rb.velocity.x > -JumpingHorizontalUpMaxSpeed)
                    {
                        rb.AddForce(Vector2.left * JumpingHorizontalUpForce);
                    }
                }
                else
                {
                    if (rb.velocity.x > -JumpingHorizontalDownMaxSpeed)
                    {
                        rb.AddForce(Vector2.left * JumpingHorizontalDownForce);
                    }
                }
            }
        }
        else
        {
            if (Mathf.Abs(rb.velocity.x) <= speed)
            {
                rb.velocity = new Vector2(h * speed, Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed));
            }
            else
            {
                rb.velocity = new Vector2(h * rb.velocity.x < 0 ? rb.velocity.x + 6f * h : rb.velocity.x, Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed));
            }
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            m_bJumpingWindow = false;
        }
        if(rb.velocity.y != 0 )
        {
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
            {
                m_bJumpRelease = true;
                //float fCurVelocity = Mathf.Lerp(rb.velocity.x, 0, 0.5f);
                //rb.velocity = new Vector2(fCurVelocity, rb.velocity.y);
            }
            if (m_bJumpRelease == true)
            {
                if(m_stateMgr.GetPlayerState() != PlayerStateDefine.PlayerState_Typ.playerState_ChangingSpeed)
                {
                    float fCurVelocity = Mathf.Lerp(rb.velocity.x, 0, JumpVelocityLerp);
                    rb.velocity = new Vector2(fCurVelocity, rb.velocity.y);
                }
            }
        }
        //log code
        if (m_fHeight < transform.position.y)
        {
            m_fHeight = transform.position.y;
        }

        //跳跃代码
        if (Input.GetKeyDown (KeyCode.W)|| Input.GetKeyDown(KeyCode.Space) ){
			if (canJump)
            {
				Jump ();
            }
			else {
                cachedJump = true;
                StartCoroutine(CacheJump());
            }
        }

		if (isTouchingGround) {
			anim.SetBool ("Jumping", false);
			legAnim.SetBool ("Jumping", false);
		} else {
			anim.SetBool ("Jumping", true);
			legAnim.SetBool ("Jumping", true);
		}
		

        // 左键子弹时间
        if (Input.GetMouseButton(0) && currWaitTime >= waitTime)
        {
            Time.timeScale = Mathf.Min(Time.timeScale, dash.reducedTimeScale);
            Time.fixedDeltaTime = dash.reducedTimeScale * startDeltaTime;
            targetDeltaTime = Time.fixedDeltaTime;
            targetTimeScale = Time.timeScale;

            //
           

        }

        if (Input.GetMouseButtonUp(0))
        {
            currWaitTime = 0;
            Time.timeScale = 1f;
            targetTimeScale = 1f;
            Time.fixedDeltaTime = startDeltaTime;
            targetDeltaTime = Time.fixedDeltaTime;
        }
        // 左键子弹时间结束

        //处理按下的指示器
        if (Input.GetMouseButton (0)) {
			if (useLineRenderer) {
				lr.enabled = true;
				HandleLineRenderer ();
			}

			IncreaseBulletSpeed ();

		} else
		if (Input.GetMouseButtonUp (0)) {
			anim.SetTrigger ("Shot");
			anim.SetBool ("IsCharging", false);
			chargeCounter = 0;
			//bulletSpeed = minBulletSpeed;
			StartCoroutine (RestoreTimeScale (0.035f));

			if (useLineRenderer) {
				lr.startColor = Color.black;
				lr.enabled = false;
			}

			Shoot ();
		}

		//双重交换
		if (Input.GetKeyDown (KeyCode.E) && doubleSwap) {
			doubleSwap = false;
			swap.Do ();
		}

		//冲刺触发

		// 动量指示器
		HandlePointer ();
		// 转向动画
		FlipFace ();
		// 找到离鼠标最近单位
		HandleObjectDistance ();
		// coyote
		HandleJump ();
	}

	void Jump () {
        box.sharedMaterial = slipperyMat;
        rb.velocity = new Vector2 (rb.velocity.x, jumpSpeed);
        canJump = false;
        m_fCurrentKeepJumping = 0.0f;
        m_fTotalForce = 0.0f;
        m_bJumpingWindow = true;
        m_bJumpRelease = false;

        m_fHeight = transform.position.y;
    }

    bool FourCornerHit()
    {
        bool res = false;
        lockedOnObjectLine.SetPosition(0, transform.position);
        lockedOnObjectLine.SetPosition(1, closestObjectToCursor.transform.position);
        BoxCollider2D targetBox = closestObjectToCursor.GetComponent<BoxCollider2D>();
        float targetX = targetBox.size.x;
        float targetY = targetBox.size.y;
        RaycastHit2D hit0 = Physics2D.Raycast(transform.position, (closestObjectToCursor.transform.position - transform.position).normalized, shootDistance, 1 << 10 | 1 << 12 | 1 << 8);
        //RaycastHit2D hit1 = Physics2D.Raycast(transform.position, (closestObjectToCursor.transform.position + new Vector3(targetX, targetY, 0f) - transform.position).normalized, shootDistance, 1 << 10 | 1 << 12 | 1 << 8);
        //RaycastHit2D hit2 = Physics2D.Raycast(transform.position, (closestObjectToCursor.transform.position + new Vector3(targetX, -targetY, 0f) - transform.position).normalized, shootDistance, 1 << 10 | 1 << 12 | 1 << 8);
        //RaycastHit2D hit3 = Physics2D.Raycast(transform.position, (closestObjectToCursor.transform.position + new Vector3(-targetX, targetY, 0f) - transform.position).normalized, shootDistance, 1 << 10 | 1 << 12 | 1 << 8);
        //RaycastHit2D hit4 = Physics2D.Raycast(transform.position, (closestObjectToCursor.transform.position + new Vector3(-targetX, -targetY, 0f) - transform.position).normalized, shootDistance, 1 << 10 | 1 << 12 | 1 << 8);

        lockedOnObjectLine.startWidth = 1f;
        swap.col = null;
        if (hit0.collider == targetBox)// || hit1.collider == targetBox || hit2.collider == targetBox || hit3.collider == targetBox || hit4.collider == targetBox)
        {
            swap.col = closestObjectToCursor.GetComponent<BoxCollider2D>();
            lockedOnObjectLine.startWidth = 5f;
            res = true;
        }
        if (swap.col && Vector3.Distance(swap.col.transform.position, transform.position) > shootDistance)
        {
            lockedOnObjectLine.startWidth = 0f;
            res = false;
            swap.col = null;
        }
        return res;
    }

	// 计算与鼠标和玩家最近的物体
	void HandleObjectDistance () {

		closestDistance = Mathf.Infinity;
		closestPlayerDistance = Mathf.Infinity;

		closestObjectToCursor = null;
		closestObjectToPlayer = null;

		foreach (var thing in thingList) {
            if(thing != null)
            {
                float distanceToCursor = Vector2.Distance(((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition)), (Vector2)thing.transform.position);
                float distanceToPlayer = Vector2.Distance((Vector2)transform.position, (Vector2)thing.transform.position);

                if (!thing.dead && distanceToCursor < closestDistance && distanceToCursor < cursorSnapThreshold && thing.enabled == true && !thing.hasShield)
                {
                    RaycastHit2D hit0 = Physics2D.Raycast(transform.position, (thing.transform.position - transform.position).normalized, shootDistance, 1 << 10 | 1 << 12 | 1 << 8);
                    if (hit0.collider == thing.gameObject.GetComponent<BoxCollider2D>())
                    {
                        closestDistance = distanceToCursor;
                        closestObjectToCursor = thing.gameObject;
                    }
                    
                }

                if (!thing.dead && distanceToPlayer < closestPlayerDistance)
                {
                    closestPlayerDistance = distanceToPlayer;
                    closestObjectToPlayer = thing.gameObject;
                }
            }
		}

		// 记号圆圈
		if (closestObjectToCursor != null) {
			marker.transform.position = new Vector3 (closestObjectToCursor.transform.position.x, closestObjectToCursor.transform.position.y, -1f);
            FourCornerHit();
            if (locked)
            {
                lockedOnObjectLine.SetPosition(0, transform.position);
                lockedOnObjectLine.SetPosition(1, swap.col.transform.position);
            }
        } else {
			marker.transform.position = new Vector3 (-10000f, 0f, 0f);
            lockedOnObjectLine.SetPosition(0,Vector3.zero);
            lockedOnObjectLine.SetPosition(1,Vector3.zero);
        }
		if (swap.col != null && doubleSwap && !swap.col.GetComponent<Thing> ().dead) {
			targetMarker.transform.position = new Vector3 (swap.col.transform.position.x, swap.col.gameObject.transform.position.y, -1f);
		} else {
			targetMarker.transform.position = new Vector3 (-10000f, 0f, 0f);
		}

	}

	void HandlePointer () {
		Vector2 rbNormal = rb.velocity.normalized;
		if (Time.timeScale == 1f || rbNormal == Vector2.zero) {
			pointer.GetComponent<SpriteRenderer> ().enabled = false;
			return;
		}
		pointer.GetComponent<SpriteRenderer> ().enabled = true;
		float angle = Vector2.SignedAngle (Vector2.right, rbNormal);
		//Debug.Log (angle);
		pointer.transform.rotation = Quaternion.Euler (0f, 0f, angle);
	}

	void FixedUpdate () {
        if(m_bJumpingWindow == true)
        {
            m_fCurrentKeepJumping += Time.fixedDeltaTime;
            if(m_fCurrentKeepJumping <= JumpAddForceTime)
            {
                rb.AddForce(transform.up * Time.fixedDeltaTime * jumpForceAir);
            }
            else
            {
                m_bJumpingWindow = false;
            }

        }
        // 左键子弹时间
        if (Input.GetMouseButton(0))
            currWaitTime += 1;

        if (Input.GetMouseButton (0)) {

			anim.SetBool ("IsCharging", true);

			chargeCounter += 1;
			if (chargeCounter > 25f) {
				if (useLineRenderer) {
					lr.startColor = Color.red;
				}

				chargeCounter = 0;

			} else if (chargeCounter > 15f) {
				// Time.timeScale = 0.1f;
				// targetTimeScale = 0.1f;
				// Time.fixedDeltaTime = startDeltaTime * 0.1f;
				// targetDeltaTime = startDeltaTime * 0.1f;
				//TODO:之后会写成渐变的感觉
				//PostEffect.SetActive (true);
			}
		} else {
			chargeCounter = 0;
		}
	}

	void HandleLineRenderer () {
		lr.SetPosition (0, transform.position);
		Vector2 mousePosition = (Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position);
		lr.SetPosition (1, transform.position + (Vector3) mousePosition.normalized * 9999);
	}

	IEnumerator RestoreTimeScale (float duration) {
		yield return new WaitForSeconds (duration);
		if (!Input.GetMouseButton (0)) {
			// targetTimeScale = 1f;
			// targetDeltaTime = startDeltaTime;
		}

	}

	void HandleRewind () {

		return;

		if (Rewind.Instance != null) {
			if (Input.GetKey (KeyCode.Space)) {

				Rewind.Instance.isReverting = true;
			} else {
				Rewind.Instance.Record ();
				Rewind.Instance.isReverting = false;
			}

			if (!Rewind.Instance.isReverting) { //TODO: if no char or mushroom is moving, don't record

			} else {
				if (Rewind.Instance.states.Count == 0) {
					return;
				}
				Rewind.Instance.Revert ();
			}
		}
	}

	void HandleShootDistanceAndLight () {
		shootDistanceLight.pointLightOuterRadius = shootDistance;
	}

	void IncreaseBulletSpeed () {
		if (chargeFrame > startChargeFrame)
			bulletSpeed = maxBulletSpeed;
		else
			chargeFrame += 1;
	}

    // 锁定目标
    void LockObject()
    {
        if (!FourCornerHit()) return;
        locked = true;

    }

    //狙击枪，点击直线瞬间交换；
    void HandleLaserChange () {
		Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        if (!closestObjectToCursor) return;
        FourCornerHit();

        swap.Do ();
        

		StartCoroutine (laserLine ());
	}

    void HandleLockLaser()
    {
        if (!locked)
            LockObject();
        else
        {
            locked = false;
            swap.Do();
          
        }
            
    }

	//狙击枪的弹道
	IEnumerator laserLine () {
		lr.enabled = true;
		lr.SetPosition (0, transform.position);
		lr.SetPosition (1, transform.position + (closestObjectToCursor.transform.position - transform.position).normalized * shootDistance);
		yield return new WaitForSeconds (0.3f);
		lr.enabled = false;
	}

	//龙王，直接点击直接交换
	void HandleChangeDirectly () {
		if (!closestObjectToCursor) return;
		swap.col = closestObjectToCursor.GetComponent<BoxCollider2D> ();
		swap.Do ();
	}

	void Shoot () {

		if (ClickChangeDirectly) {
			HandleChangeDirectly ();
			return;
		}

		if (laserBullet) {
			HandleLaserChange ();
			return;
		}

        if (lockLaserBullet)
        {
            HandleLockLaser();
            return;
        }

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		GameObject newBullet = Instantiate (bullet, transform.position + ((Vector3) mouseWorldPos - transform.position).normalized * 30f, Quaternion.Euler (0, 0, -AngleBetween (Vector2.left, ((Vector2) mouseWorldPos - (Vector2) transform.position).normalized)));

		if (isHomingBullet && closestObjectToCursor!=null)
		{
			newBullet.GetComponent<Bullet> ().SetHomingBullet(closestObjectToCursor.transform,homingBulletRotateSpeed,homingBulletSpeed);
			return;
		}

		//修改Bullet的动画
		if (bulletSpeed == maxBulletSpeed) {
			newBullet.GetComponent<Bullet> ().SetBulletType (Bullet.BulletType.fast);
		} else newBullet.GetComponent<Bullet> ().SetBulletType (Bullet.BulletType.slow);

		Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D> ();

		if (closestObjectToCursor)
			bulletBody.velocity = ((Vector2) closestObjectToCursor.transform.position - (Vector2) transform.position).normalized * bulletSpeed;
		else
			bulletBody.velocity = (mouseWorldPos - (Vector2) transform.position).normalized * bulletSpeed;

		bulletSpeed = minBulletSpeed;
		chargeFrame = 0;

	}

    IEnumerator BloodEffect()
    {
        blood.color = new Color(1f, 1f, 1f, 0f);
        float curr = 0f;
        while (curr < 0.4f)
        {
            curr = Mathf.Clamp01(curr + Time.unscaledDeltaTime / bloodEffectDuration);
            blood.color = new Color(1f, 1f, 1f, curr);
            yield return new WaitForEndOfFrame();
        }
        while (curr > 0f)
        {
            curr = Mathf.Clamp01(curr - Time.unscaledDeltaTime / bloodEffectDuration);
            blood.color = new Color(1f, 1f, 1f, curr);
            yield return new WaitForEndOfFrame();
        }
    }

    public override void Die()
    {
        if (invincible) return;
        StartCoroutine(BloodEffect());
        StartCoroutine(OnHit());
        if (hp > 1)
        {
            hp -= 1;
            return;
        }
        StartCoroutine(DelayRestart());

        active = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.enabled = false;
        }
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<HeadBodySeparation>().PlayerDead(25000);

        //transform.localScale = Vector3.zero;

    
    }
    private void _delayAction()
    {

    }
    public IEnumerator DelayRestart()
    {
        yield return new WaitForSeconds(1f);
        //StartCoroutine(DelayLoadScene());
        m_playDieAction.Invoke(this);
    }

    public IEnumerator DelayLoadScene()
    {
        yield return new WaitForSeconds(0.25f);
        Time.fixedDeltaTime = startDeltaTime;
        Time.timeScale = 1f;
        if (levelTest)
            levelTest.AddDeadNum(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
	public override void Revive () {
		active = true;
		GetComponent<BoxCollider2D> ().enabled = true;
		GetComponent<SpriteRenderer> ().enabled = true;
		GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic;
		foreach (var sr in GetComponentsInChildren<SpriteRenderer> ()) {
			sr.enabled = true;
		}
		//transform.localScale = originalScale;
	}

	private void FlipFace () {
		Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		if (mouseWorldPos.x > transform.position.x) {
			spriteRenderer.flipX = true;
		} else spriteRenderer.flipX = false;
	}

	//李昊明的数学公式计算*1
	public static Vector3 RotatePointAroundPivot (Vector3 point, Vector3 pivot, float angle) {
		angle = angle * (Mathf.PI / 180f);
		var rotatedX = Mathf.Cos (angle) * (point.x - pivot.x) - Mathf.Sin (angle) * (point.y - pivot.y) + pivot.x;
		var rotatedY = Mathf.Sin (angle) * (point.x - pivot.x) + Mathf.Cos (angle) * (point.y - pivot.y) + pivot.y;
		return new Vector3 (rotatedX, rotatedY, 0);
	}

	//李昊明的数学公式计算*2
	public static float AngleBetween (Vector2 vectorA, Vector2 vectorB) {
		float angle = Vector2.Angle (vectorA, vectorB);
		Vector3 cross = Vector3.Cross (vectorA, vectorB);

		if (cross.z > 0) {
			angle = 360 - angle;
		}

		return angle;
	}

    private void _logHeight()
    {
        if(canJump == false)
        {
            //Debug.Log(string.Format("Most Height is {0} force is {1}", m_fHeight, m_fTotalForce));
        }
    }

	void HandleJump () {
        if (!isTouchingGround)
            StartCoroutine (JumpTolerence ());
        else
        {
            if( m_bJumpingWindow == false)
            {
                _logHeight();
                canJump = true;
                m_stateMgr.SetPlayerState(PlayerStateDefine.PlayerState_Typ.PlayerState_None);
                m_bJumpRelease = false;
            }
            else
            {
                //if (m_fCurrentKeepJumping > Jump2ndTime)
                //{
                //    canJump = true;
                //}
            }
        }
	}

	IEnumerator JumpTolerence () {
		int curr = 0;
		while (curr <= coyoteTime) {
			if (isTouchingGround) {

                _logHeight();
                canJump = true;
                m_stateMgr.SetPlayerState(PlayerStateDefine.PlayerState_Typ.PlayerState_None);
                m_bJumpRelease = false;
                yield return null;
			}

			yield return new WaitForEndOfFrame ();
			curr++;
		}
		canJump = false;
	}

	IEnumerator CacheJump () {
		int curr = 0;
		while (curr <= cacheJumpTime) {
			if (canJump) {
				Jump ();
				cachedJump = false;
				yield return null;
			}

			yield return new WaitForEndOfFrame ();
			curr++;
		}
		cachedJump = false;
	}

    IEnumerator OnHit()
    {
        invincible = true;
        Color hitColor = new Color(1f, 0.4f, 0f);
        CameraShaker.Instance.ShakeOnce(40f, 5f, 0.1f, 0.1f);

        GetComponent<SpriteRenderer>().color = hitColor;
        yield return new WaitForSeconds(0.3f);
        GetComponent<SpriteRenderer>().color = Color.white;
        invincible = false;
    }
    public void SetColShadow()
    {
        if (isPlayColShadow || !swap.col)
            return;
        isPlayColShadow = true;
        colShadow.enabled = true;
        colShadow.sprite = spriteRenderer.sprite; //swap.col.GetComponent<SpriteRenderer>().sprite;
        //colShadow.transform.localScale = swap.col.transform.localScale;
        colShadow.flipX = spriteRenderer.flipX;
        StartCoroutine(PlayColShadow());
    }
    IEnumerator PlayColShadow()
    {
        colShadow.transform.position = transform.position;
        while (Input.GetMouseButton(1) || Input.GetMouseButton(0)/*&& swap.delaying*/)
        {
            if ((Vector2)lr.GetPosition(5) != Vector2.zero)
            {
                //Debug.Log(Time.timeScale);
                colShadow.transform.position = Vector2.Lerp(colShadow.transform.position, lr.GetPosition(5), Time.deltaTime * 50);

                if ((lr.GetPosition(5) - colShadow.transform.position).magnitude < 100)
                    colShadow.color = Color.Lerp(colShadow.color, new Color(1, 1, 1, 0),Time.deltaTime*50);
            }
            if ((lr.GetPosition(5) - colShadow.transform.position).magnitude < 20)
            {
                colShadow.transform.position = transform.position;
                colShadow.color = new Color(1, 1, 1, 100/255f);
            }
            yield return null;
        }
        colShadow.enabled = false;
        colShadow.transform.position = transform.position;
       
        yield return new WaitForSeconds(0.1f);
        playerShadow.sprite = spriteRenderer.sprite;
        playerShadow.flipX = spriteRenderer.flipX;
        playerShadow.color = new Color(0, 0, 0, 100 / 255f);
     
        for (int i = 0; i < 4; i++)
        {
            if(swap == null ||(swap.col == null))
            {

            }
            else
            {
                SpriteRenderer s = Instantiate(playerShadow, swap.col.transform.position, Quaternion.identity);
                s.enabled = true;
                s.GetComponent<AutoDestroy>().StartDestroy(0.5f + i / 10f);
                yield return new WaitForSeconds(0.04f);
            }
        }
        isPlayColShadow = false;
    }
}