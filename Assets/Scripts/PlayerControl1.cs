using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EZCameraShake;
using UnityEngine.UI;
using UnityEngine.Events;
using Rewired;
using Com.LuisPedroFonseca.ProCamera2D;

//using UnityEngine.Rendering.LWRP;

public class PlayerControl1 : PlayerControl {


    public Overhead overhead;
    public static PlayerControl1 Instance { get; private set; }
    //Rewired------------------------------------------------------------


    public enum ControlWay
    {
        isKeyboard = 0,
        isJoystick = 1,
        isMobile = 2,
    }
    public ControlWay controlState = ControlWay.isKeyboard;

    public int playerId = 0;
    public Player player;

    [Tooltip("处理角色在空中时候，平行速度迅速递减的lerp值")]
    [Range(0, 1)]
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
    [Header("子弹参数")]
    public GameObject bullet;




    [Header("点击直接瞬间交换，不会被阻挡")]
    public bool ClickChangeDirectly;
    [Header("按键切换目标")]
    public bool toggleSwapTarget = false;
    public GameObject toggleTarget;
    public List<GameObject> swappable;
    public int index;
    public float DelaySwitchTime;
    [Header("激光枪射击，以角度计算锁定目标")]
    public bool laserBulletAngle = false;
    public float aimAngle;
    [Header("激光枪射击，瞬间交换，会被阻挡")]
    public bool laserBullet = false;
    [Header("lock first, then 激光枪射击，瞬间交换，会被阻挡")]
    public bool lockLaserBullet = false;
    [Header("带有跟踪效果")]
    public bool isHomingBullet = false;
    public float homingBulletSpeed = 10f;
    public float homingBulletRotateSpeed = 200f;

    [Header("带有射击距离限制")]
    public bool hasShootDistance = true;
    //按照玩家的距离来计算
    public bool countDistanceToPlayer = true;
    public float shootDistance = 500f;

    [SerializeField]
    UnityEngine.Experimental.Rendering.LWRP.Light2D shootDistanceLight;



    [Space]

    [Header("子弹速度")]
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

    [Header("瞄准表现")]
    public float fourCornerScanMargin = 5f;
    public bool useLineRenderer = false;
    public bool useCursor = false;
    public LineRenderer lr;
    GameObject cursor;
    public GameObject cursorPrefab;
    bool locked;
    public Rigidbody2D rb;

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
    public GameObject FirstJumpEffect;

    public GameObject MoveSmokeEffect;

    public Vector3 originalScale;

    public Animator legAnim;
    public SpriteRenderer legsSpriteRenderer;
    public SpriteRenderer spriteRenderer;

    private int chargeCounter = 0;

    SpriteRenderer blackSr;
    bool leftPressed;

    public GameObject pointer;
    public GameObject swapTarget;

    public List<Thing> thingList;
    public GameObject prevClosestObjectToCursor;
    public GameObject closestObjectToCursor;
    private GameObject TempObjectToCursor;
    private float m_fTickWaitCursorTime;
    public float WaitCursorTime;
    public float WaitCursorTimeForKeyboard;
    private GameObject cacheCursorTarget = null;
    public GameObject closestObjectToPlayer;
    public float closestDistance = Mathf.Infinity;
    public float closestPlayerDistance = Mathf.Infinity;
    public float cursorSnapThreshold;
    public GameObject marker;

    private PlayerMarkerComponent m_marker;

    public Swap swap;
    public Dash dash;

    public LayerMask TouchLayer;
    public LayerMask LayerForLockObject;
    public LayerMask BoxLayer;
    public LayerMask MovePlatformLayer;

    [Tooltip("空中跳跃施加力的时间")]
    public float JumpAddForceTime;
    private float m_fCurrentKeepJumping;
    private float m_fTotalForce;

    private float m_fHeight;

    public SpriteRenderer colShadow;
    public SpriteRenderer playerShadow;

    public float PlayerSpawnTime;
    private Vector3 swapColPosition;
    private bool isPlayColShadow = false;

    private LevelTest levelTest;

    AudioSource audioSource;
    public AudioClip jumpClip;
    public AudioClip hitClip;

    public void InitSkills() {
        swap = GetComponent<Swap>();
        dash = GetComponent<Dash>();
    }

    public LineRenderer lockedOnObjectLine;

    private PlayerStateManager m_stateMgr;

    private UnityAction<PlayerControl1> m_playDieAction;

    private Color trajectoryStartColor;
    private Color trajectoryEndColor;
    private bool trajectoryOn = true;

    //Rewired--------------------------------------------------手柄震动
    [Header("下落手柄震动")]
    public float landingMotor1Level;
    public float landingMotor2Level;
    public float landingMotor1duration;
    public float landingMotor2duration;

    [Header("死亡手柄震动")]
    public float dieMotor1Level;
    public float dieMotor2Level;
    public float dieMotor1duration;
    public float dieMotor2duration;

    [Header("受伤手柄震动")]
    public float onHitMotor1Level;
    public float onHitMotor2Level;
    public float onHitMotor1duration;
    public float onHitMotor2duration;

    public float DashingMoveTime;

    private bool m_bDashRequest;

    private bool m_bDashMove;
    private bool m_bDashResult;
    private float m_fCurDashDuration;
    private bool m_bDashing;

    private Vector3 m_vecMouseWorldPos;

    private BulletTime m_bulletTime;
    private PlayerDoubleSwap m_doubleSwap;

    float wallCheckBoxWidth = 50f;
    float wallCheckBoxIndent = 2f;

    Vector2 wallCheckTopLeft;
    Vector2 wallCheckBottomRight;
    Vector2 leftWallCheckTopLeft;
    Vector2 leftWallCheckBottomRight;

    Vector2 upWallCheckTopLeft;
    Vector2 upWallCheckBottomRight;
    Vector2 floorCheckTopLeft;
    Vector2 floorCheckBottomRight;

    public bool wallJump;
    public Animator anim;

    public PlayerAnimationComponent m_aniCom;
    public PlayerBoosty PlayerBoostyAttr;
    public float DistanceOfPlayerAndThing;
    private EnemyDieSound m_enemySound;

    public bool disableAirControl;

    void Awake()
    {
        overhead = GetComponent<Overhead>();
        if (HasRepawnPoint)
        {
            if (CheckPointTotalManager.instance != null)
            {
                transform.position = CheckPointTotalManager.instance.GetPlayerPos();
            }
        }
        if (ProCamera2D.Exists == true)
        {
            ProCamera2D.Instance.AddCameraTarget(transform);
        }

        dash.RegisteDashEvent(_dashStart, _dashOver);
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        //Rewired------------------------------------------------------------
        if (ReInput.players != null)
        {
            player = ReInput.players.GetPlayer(playerId);
        }

        //手柄死区
        if (controlState == ControlWay.isJoystick)
        {
            if (player.controllers.Joysticks.Count > 0)
            {
                player.controllers.Joysticks[0].calibrationMap.GetAxis(0).deadZone = 0.5f;
            }
        }

        GlobalVariable.SetPlayer(this);
        originalScale = transform.localScale;
        startDeltaTime = Time.fixedDeltaTime;
        targetDeltaTime = startDeltaTime;
        targetTimeScale = 1f;
        rb = GetComponent<Rigidbody2D>();

        //PlayerBoostyAttr = new PlayerBoosty(rb);
        PlayerBoostyAttr.SetBoostyData(rb, player);
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
        box = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        swappable = new List<GameObject>();
    }

    void Start()
    {
        m_enemySound = GetComponent<EnemyDieSound>();
        m_marker = marker.GetComponent<PlayerMarkerComponent>();
        if (controlState == ControlWay.isJoystick)
        {
            laserBulletAngle = true;
        }
        m_doubleSwap = GetComponent<PlayerDoubleSwap>();
        m_vecMouseWorldPos = new Vector3();
        GameObject objLevelMgr = GameObject.FindGameObjectWithTag("LevelManager");

        if (objLevelMgr != null)
        {
            levelTest = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelTest>();
        }
        //m_playDieAction += GlobalVariable.GetUIPlayerCtrl().PlayerDieAction;
        m_stateMgr = GetComponent<PlayerStateManager>();
        lockedOnObjectLine.startWidth = 1f;
        lockedOnObjectLine.positionCount = 2;

        blackSr = GameObject.FindWithTag("black").GetComponent<SpriteRenderer>();
        bulletSpeed = minBulletSpeed;

        InitSkills();


        //设置射程和灯光
        if (hasShootDistance) HandleShootDistanceAndLight();
        hp = maxhp;

        trajectoryStartColor = lr.startColor;
        trajectoryEndColor = lr.endColor;

        m_bulletTime = GetComponent<BulletTime>();

        InitWallChecks();
        
    }

    #region wallTouchChecks
    void InitWallChecks()
    {
        wallCheckTopLeft = new Vector2
                         (
                            (box.size.x / 2f - wallCheckBoxWidth / 2f),
                            (box.size.y / 2f - wallCheckBoxIndent)
                         );
        wallCheckBottomRight = new Vector2
                                 (
                                    box.size.x / 2f + wallCheckBoxWidth / 2f,
                                    -(box.size.y / 2f - wallCheckBoxIndent)
                                 );
        leftWallCheckTopLeft = new Vector2
                         (
                            -(box.size.x / 2f + wallCheckBoxWidth / 2f),
                            (box.size.y / 2f - wallCheckBoxIndent)
                         );
        leftWallCheckBottomRight = new Vector2
                                 (
                                    -(box.size.x / 2f - wallCheckBoxWidth / 2f),
                                    -(box.size.y / 2f - wallCheckBoxIndent)
                                 );
        upWallCheckTopLeft = new Vector2
                         (
                            -(box.size.x / 2f - wallCheckBoxIndent),
                            box.size.y / 2f + wallCheckBoxWidth / 2f
                         );
        upWallCheckBottomRight = new Vector2
                                 (
                                    box.size.x / 2f - wallCheckBoxIndent,
                                    box.size.y / 2f - wallCheckBoxWidth / 2f
                                 );
        floorCheckTopLeft = new Vector2
                         (
                            -(box.size.x / 2f - wallCheckBoxIndent),
                            -(box.size.y / 2f - wallCheckBoxWidth / 2f)
                         );
        floorCheckBottomRight = new Vector2
                                 (
                                    box.size.x / 2f - wallCheckBoxIndent,
                                    -(box.size.y / 2f + wallCheckBoxIndent)
                                 );
    }
    public bool touchingWallRight()
    {
        return Physics2D.OverlapArea
                (
                    (Vector2)transform.position + wallCheckTopLeft,
                    (Vector2)transform.position + wallCheckBottomRight,
                    TouchLayer
                );
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
        return Physics2D.OverlapArea
                (
                    (Vector2)transform.position + floorCheckTopLeft,
                    (Vector2)transform.position + floorCheckBottomRight,
                    TouchLayer
                );
    }
    #endregion

    public void RegisteDieAction(UnityAction<PlayerControl1> _act)
    {
        m_playDieAction += _act;
    }
    public void UnregisteDieAction(UnityAction<PlayerControl1> _act)
    {
        m_playDieAction -= _act;
    }

    private void OnDestroy()
    {
        //GlobalVariable.GetUIPlayerCtrl().UnregisteDelayRestart(_delayAction);
        if (GlobalVariable.GetUIPlayerCtrl() != null)
        {
            //m_playDieAction -= GlobalVariable.GetUIPlayerCtrl().PlayerDieAction;
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

    void Update() {
        if (wallJump)
            isTouchingGround = Physics2D.OverlapArea
                (
                    (Vector2)transform.position + wallCheckTopLeft,
                    (Vector2)transform.position + wallCheckBottomRight,
                    TouchLayer
                )
                ||
                Physics2D.OverlapArea
                (
                    (Vector2)transform.position + leftWallCheckTopLeft,
                    (Vector2)transform.position + leftWallCheckBottomRight,
                    TouchLayer
                );
        //print(rb.velocity.y);
        anim.SetFloat("SpeedY", rb.velocity.y / 2);
        anim.SetFloat("SpeedX", rb.velocity.x / 2);
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


        isTouchingGround = (_ray1 | _ray2 | _ray3 | _ray4 | _ray5) || (wallJump ? isTouchingGround : false);

        if (isTouchingGround == true && !wallJump)
        {
            isTouchingGround = _isTouching(ref _ray1) | _isTouching(ref _ray2) | _isTouching(ref _ray3) | _isTouching(ref _ray4) | _isTouching(ref _ray5);
        }

        if (isTouchingGround == true)
        {
            m_bDashing = false;
            dash.PlayerTouchGround();
        }
        // landing
        if (isTouchingGround != isGroundTemp && isTouchingGround == true && landingParticle != null)
        {
            //Rewired 手柄震动---------------------------------------------------
            PlayerControl1.Instance.player.SetVibration(0, landingMotor1Level, landingMotor1duration);
            PlayerControl1.Instance.player.SetVibration(1, landingMotor2Level, landingMotor2duration);

            GameObject part = Instantiate(landingParticle, transform.position - Vector3.up * 10, Quaternion.identity);
            Destroy(part, 2f);
            //if (controlState==ControlWay.isKeyboard && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && Mathf.Abs(rb.velocity.y) <= 5f)
            //{

            //    rb.velocity = Vector2.zero;

            //}

            ////Rewired------------------------------------------------------------
            //if (controlState == ControlWay.isJoystick && (player.GetAxisRaw("MoveHorizontal") == 0)
            //    || controlState == ControlWay.isMobile && (player.GetAxisRaw("MoveHorizontal") == 0))
            //{
            //    rb.velocity = Vector2.zero;
            //}
            //if ((player.GetAxisRaw("MoveHorizontal") == 0))
            //{
            //    rb.velocity = Vector2.zero;
            //}


        }

        box.sharedMaterial = isTouchingGround ? roughMat : slipperyMat;

        isGroundTemp = isTouchingGround;

        HandleRewind();


        //Rewired------------------------------------------------------------
        if (player.GetButtonDown("Restart")) {

            Time.fixedDeltaTime = startDeltaTime;
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        //if (!Rewind.Instance.isReverting) {
        //	Time.timeScale = Mathf.Clamp (Time.timeScale + ((targetTimeScale >= Time.timeScale) ? 0.04f : -0.04f), 0.1f, 1f);
        //	Time.fixedDeltaTime = Mathf.Clamp (Time.fixedDeltaTime + ((targetDeltaTime >= Time.fixedDeltaTime) ? 0.04f * startDeltaTime : -0.04f * startDeltaTime), 0.1f * startDeltaTime, startDeltaTime);
        //}

        Time.timeScale = Mathf.Clamp(Time.timeScale + ((targetTimeScale >= Time.timeScale) ? 0.07f : -0.07f), 0.01f, 1f);
        Time.fixedDeltaTime = Mathf.Clamp(Time.fixedDeltaTime + ((targetDeltaTime >= Time.fixedDeltaTime) ? 0.07f * startDeltaTime : -0.07f * startDeltaTime), 0.01f * startDeltaTime, startDeltaTime);

        if (!active)
            return;


        //左右移动
        //float h = (Input.GetKey(KeyCode.D) ? 1 : 0) + (Input.GetKey(KeyCode.A) ? -1 : 0);
        float h = 0.0f;
        //Rewired------------------------------------------------------------
        //h += (player.GetAxis("MoveHorizontal") > 0.2f ? 1 : 0) + (player.GetAxis("MoveHorizontal") < -0.2f ? -1 : 0);
        h += (player.GetAxisRaw("MoveHorizontal") > 0.2f ? 1 : 0) + (player.GetAxisRaw("MoveHorizontal") < -0.2f ? -1 : 0);

        if (Mathf.Abs(h) > 0) {
            m_bJumpRelease = false;

            anim.SetBool("Moving", true);
            if (legAnim != null)
            {
                legAnim.SetBool("Moving", true);
            }
            if (h > 0) legsSpriteRenderer.flipX = true;
            else legsSpriteRenderer.flipX = false;
        } else
        {
            m_bJumpRelease = false;
            anim.SetBool("Moving", false);
            if (legAnim != null && legAnim.gameObject.activeInHierarchy == true)
            {
                legAnim.SetBool("Moving", false);
            }
        }

        //if (Mathf.Abs(rb.velocity.x) <= speed)
        //{
        //    rb.velocity = new Vector2(h * speed, Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed));
        //}
        //else
        //{
        //    rb.velocity = new Vector2(h * rb.velocity.x < 0 ? rb.velocity.x + 6f * h : rb.velocity.x, Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed));
        //}
        if (disableAirControl && !isTouchingGround)
        {
            h = 0;
            Debug.Log("no");
        }
            
        if (canJump == false && !disableAirControl)
        {

            if (h > 0)
            {
                if (rb.velocity.y > 0)
                {
                    if (rb.velocity.x < JumpingHorizontalUpMaxSpeed)
                    {
                        rb.AddForce(JumpingHorizontalUpForce * new Vector2(h, 0));
                    }
                }
                else
                {
                    if (rb.velocity.x < JumpingHorizontalDownMaxSpeed)
                    {
                        rb.AddForce(JumpingHorizontalDownForce * new Vector2(h, 0));
                    }
                }
            }
            else if (h < 0)
            {

                if (rb.velocity.y > 0)
                {
                    if (rb.velocity.x > -JumpingHorizontalUpMaxSpeed)
                    {
                        rb.AddForce(JumpingHorizontalUpForce * new Vector2(h, 0));
                    }
                }
                else
                {
                    if (rb.velocity.x > -JumpingHorizontalDownMaxSpeed)
                    {
                        rb.AddForce(JumpingHorizontalDownForce * new Vector2(h, 0));
                    }
                }
            }
        }
        else
        {
            if (Mathf.Abs(rb.velocity.x) <= speed && dash.isDashing == false)
            {
                rb.velocity = new Vector2(h * speed, Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed));
            }
            else
            {
                rb.velocity = new Vector2(h * rb.velocity.x < 0 ? rb.velocity.x + 6f * h : rb.velocity.x, Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed));
            }
        }
        PlayerBoostyAttr.BoostyProcess();
        if (PlayerBoostyAttr.IsBoosty() == false)
        {
        }
        else
        {
            PlayerBoostyAttr.Update(h);
        }
        if (h == 0.0f && dash.isDashing == false && isTouchingGround)
        {
            //rb.velocity = new Vector2(0, Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed));
        }


        //Rewired------------------------------------------------------------
        //if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space) || player.GetButtonUp("Jump"))
        if (player.GetButtonUp("Jump"))
        {
            m_bJumpingWindow = false;
        }
        if (rb.velocity.y != 0)
        {
            //Rewired------------------------------------------------------------
            //if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A) || (player.GetAxisRaw("MoveHorizontal") == 0 && !(controlState==ControlWay.isKeyboard)))
            if (player.GetAxisRaw("MoveHorizontal") == 0)
            {
                if (m_bDashMove == true)
                {
                    //还在dash特定时间，不能把水平速度，趋近于零
                }
                else
                {
                    if (m_bDashing == true)
                    {
                        if (m_bDashResult == true)
                        {
                            m_bJumpRelease = true;
                        }
                    }
                    else
                    {
                        m_bJumpRelease = true;
                    }
                }
            }
            if ((player.GetAxisRaw("MoveHorizontal") == 0 && !(controlState == ControlWay.isKeyboard)))
            {
                if (m_bDashing == true)
                {
                    if (m_bDashResult == true)
                    {
                        m_bJumpRelease = true;
                    }
                }
            }
            if (m_bJumpRelease == true)
            {
                if (m_stateMgr.GetPlayerState() != PlayerStateDefine.PlayerState_Typ.playerState_ChangingSpeed && m_stateMgr.GetPlayerState() != PlayerStateDefine.PlayerState_Typ.playerState_Dash)
                {
                    //Debug.Log(string.Format("Playercontrol velocity to zero {0}", m_stateMgr.GetPlayerState()));
                    float fCurVelocity = Mathf.Lerp(rb.velocity.x, 0, JumpVelocityLerp);
                    //rb.velocity = new Vector2(fCurVelocity, rb.velocity.y);
                }
            }
        }
        //log code
        if (m_fHeight < transform.position.y)
        {
            m_fHeight = transform.position.y;
        }
        if (Input.GetKeyUp(KeyCode.A) && rb.velocity.x < 0f || Input.GetKeyUp(KeyCode.D) && rb.velocity.x > 0f)
            rb.velocity = new Vector2(0f, rb.velocity.y);
        //跳跃代码
        //Rewired------------------------------------------------------------
        //if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || player.GetButtonDown("Jump"))
        if (player.GetButtonDown("Jump"))
        {
            if (canJump)
            {
                Jump();
            }
            else {
                cachedJump = true;
                StartCoroutine(CacheJump());
            }
        }

        if (isTouchingGround) {
            if( m_stateMgr.GetPlayerState() != PlayerStateDefine.PlayerState_Typ.playerState_Dash && m_stateMgr.GetPlayerState() != PlayerStateDefine.PlayerState_Typ.playerState_IdleDash)
            {
                anim.SetBool("Jumping", false);
                if (legAnim != null && legAnim.gameObject.activeInHierarchy == true)
                {
                    legAnim.SetBool("Jumping", false);
                }
                m_aniCom.PlayerToIdle();
                m_stateMgr.SetPlayerState(PlayerStateDefine.PlayerState_Typ.playerState_Idle);
            }
        } else {
            if( dash.isDashing == true )
            {

            }
            else
            {
                anim.SetBool("Jumping", true);
                if (legAnim != null && legAnim.gameObject.activeInHierarchy == true)
                {
                    legAnim.SetBool("Jumping", true);
                }
            }
        }


        // 左键子弹时间
        //Rewired------------------------------------------------------------
        if ( (player.GetButton("Switch") && swap.IsSwapCoolDownValid() == true && currWaitTime >= waitTime )
            || (controlState == ControlWay.isMobile && TouchControl.Instance.aimDrag && currWaitTime >= waitTime)
            )
        {
            m_bulletTime.SetCustomizeTime(Mathf.Min(Time.timeScale, dash.reducedTimeScale), dash.reducedTimeScale * startDeltaTime);
            m_bulletTime.DelayActive(DelaySwitchTime);
            //Time.timeScale = Mathf.Min(Time.timeScale, dash.reducedTimeScale);
            //Time.fixedDeltaTime = dash.reducedTimeScale * startDeltaTime;
            targetDeltaTime = Time.fixedDeltaTime;
            targetTimeScale = Time.timeScale;
        }


        //Rewired------------------------------------------------------------
        if (player.GetButtonUp("Switch") && swap.IsSwapCoolDownValid() == true || player.GetButtonUp("QuichSwitch"))
        {
            CancelAimBulletTime();
        }

        if (player.GetButtonUp("Dash"))
        {
            DashRequestByPlayer();
        }

        // 左键子弹时间结束

        //处理按下的指示器
        //Rewired------------------------------------------------------------
        if ( player.GetButton("Switch") && swap.IsSwapCoolDownValid() == true) {
            //m_bulletTime.ActiveBulletTime(true, BulletTime.BulletTimePriority.BulletTimePriority_Low);
            //m_bulletTime.DelayActive(DelaySwitchTime);
            if (useLineRenderer) {
                //lr.enabled = true;
                HandleLineRenderer();
            }

            IncreaseBulletSpeed();

        } else //Rewired------------------------------------------------------------
    if (player.GetButtonUp("Switch") && swap.IsSwapCoolDownValid() == true) {

            m_bulletTime.ActiveBulletTime(false, BulletTime.BulletTimePriority.BulletTimePriority_Low);
            // || (isPrepareToSwitch && player.GetAxis2DRaw("DashAimHorizontal", "DashAimVertical").magnitude == 0f


            anim.SetTrigger("Shot");
            anim.SetBool("IsCharging", false);
            chargeCounter = 0;
          
            StartCoroutine(RestoreTimeScale(0.035f));

            if (useLineRenderer) {
                lr.startColor = Color.black;
                lr.enabled = false;
            }

            Shoot();

           
            
        }
        m_bDashRequest = false;

        //双重交换
        //Rewired------------------------------------------------------------
        //if (Input.GetKeyDown(KeyCode.F) && doubleSwap || player.GetButtonDown("DoubleSwap") && doubleSwap) {
        if (player.GetButtonDown("DoubleSwap") && m_doubleSwap.DoubleSwap)
        {
            //原先是通过子弹进行呼唤，所以这里需要false，但是现在情况变了，这个作为一个功能开关，而不是一个属性值
            //doubleSwap = false;
            m_doubleSwap.DoDoubleSwap();
        }

        //冲刺触发

        // 动量指示器
        HandlePointer();
        // 转向动画
        FlipFace(-rb.velocity.x);
        // 找到离鼠标最近单位
        HandleObjectDistance();
        // coyote
        HandleJump();

        HandleTrajectoryTest();
        HandleShootSlowBullet();

        HandleToggleSwapTarget();
    }
    public void DashRequestByPlayer()
    {
        dash.RequestDash();
        m_bDashRequest = true;
    }
    public void CancelAimBulletTime()
    {
        currWaitTime = 0;
        Time.timeScale = 1f;
        targetTimeScale = 1f;
        Time.fixedDeltaTime = startDeltaTime;
        targetDeltaTime = Time.fixedDeltaTime;
    }

    void Jump() {

        m_stateMgr.SetPlayerState(PlayerStateDefine.PlayerState_Typ.playerState_Jumping);
        box.sharedMaterial = slipperyMat;
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        canJump = false;
        m_fCurrentKeepJumping = 0.0f;
        m_fTotalForce = 0.0f;
        m_bJumpingWindow = true;
        m_bJumpRelease = false;

        m_fHeight = transform.position.y;

        audioSource.PlayOneShot(jumpClip);
        if(FirstJumpEffect != null)
        {
            Instantiate(FirstJumpEffect, transform.position - Vector3.up * 10, Quaternion.identity);
        }
    }

    public LayerMask GetCurrentLayerMask()
    {
        if(ClickChangeDirectly == true)
        {
            return LayerForLockObject;
        }
        else
        {
            return TouchLayer;
        }
    }

    bool Hit(GameObject target)
    {
        BoxCollider2D targetBox = target.GetComponent<BoxCollider2D>();
        float targetX = Mathf.Max(0f, targetBox.size.x / 2f - fourCornerScanMargin);
        float targetY = Mathf.Max(0f, targetBox.size.y / 2f - fourCornerScanMargin);
        //RaycastHit2D hit0 = Physics2D.Raycast(transform.position, (target.transform.position - transform.position).normalized, shootDistance, 1 << 10 | 1 << 12 | 1 << 8);
        RaycastHit2D hit0 = Physics2D.Raycast(transform.position, (target.transform.position - transform.position).normalized, shootDistance, GetCurrentLayerMask());
        //RaycastHit2D hit1 = Physics2D.Raycast(transform.position, (target.transform.position + new Vector3(targetX, targetY, 0f) - transform.position).normalized, shootDistance, 1 << 10 | 1 << 12 | 1 << 8);
        //RaycastHit2D hit2 = Physics2D.Raycast(transform.position, (target.transform.position + new Vector3(targetX, -targetY, 0f) - transform.position).normalized, shootDistance, 1 << 10 | 1 << 12 | 1 << 8);
        //RaycastHit2D hit3 = Physics2D.Raycast(transform.position, (target.transform.position + new Vector3(-targetX, targetY, 0f) - transform.position).normalized, shootDistance, 1 << 10 | 1 << 12 | 1 << 8);
        //RaycastHit2D hit4 = Physics2D.Raycast(transform.position, (target.transform.position + new Vector3(-targetX, -targetY, 0f) - transform.position).normalized, shootDistance, 1 << 10 | 1 << 12 | 1 << 8);
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, (target.transform.position + new Vector3(targetX, targetY, 0f) - transform.position).normalized, shootDistance, GetCurrentLayerMask());
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, (target.transform.position + new Vector3(targetX, -targetY, 0f) - transform.position).normalized, shootDistance, GetCurrentLayerMask());
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, (target.transform.position + new Vector3(-targetX, targetY, 0f) - transform.position).normalized, shootDistance, GetCurrentLayerMask());
        RaycastHit2D hit4 = Physics2D.Raycast(transform.position, (target.transform.position + new Vector3(-targetX, -targetY, 0f) - transform.position).normalized, shootDistance, GetCurrentLayerMask());
        return (hit1.collider == targetBox || hit2.collider == targetBox || hit3.collider == targetBox || hit4.collider == targetBox);

    }

    bool FourCornerHit()
    {
        bool res = false;
        //lockedOnObjectLine.SetPosition(1, closestObjectToCursor.transform.position);
        BoxCollider2D targetBox = closestObjectToCursor.GetComponent<BoxCollider2D>();
        float targetX = Mathf.Max(0f, targetBox.size.x / 2f - fourCornerScanMargin);
        float targetY = Mathf.Max(0f, targetBox.size.y / 2f - fourCornerScanMargin);

        Vector3 vecHit1Pos = (closestObjectToCursor.transform.position + new Vector3(targetX, targetY, 0f) - transform.position);
        Vector3 vecHit2Pos = (closestObjectToCursor.transform.position + new Vector3(targetX, -targetY, 0f) - transform.position);
        Vector3 vecHit3Pos = (closestObjectToCursor.transform.position + new Vector3(-targetX, targetY, 0f) - transform.position);
        Vector3 vecHit4Pos = (closestObjectToCursor.transform.position + new Vector3(-targetX, -targetY, 0f) - transform.position);
        RaycastHit2D hit0 = Physics2D.Raycast(transform.position, (closestObjectToCursor.transform.position - transform.position).normalized, shootDistance, 1 << 10 | 1 << 12 | 1 << 8);
        //awRaycastHit2D hit0 = Physics2D.Raycast(transform.position, new Vector2(0,0), shootDistance, 1 << 10 | 1 << 12 | 1 << 8);
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, vecHit1Pos, shootDistance, 1 << 10 | 1 << 12 | 1 << 8);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, vecHit2Pos, shootDistance, 1 << 10 | 1 << 12 | 1 << 8);
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, vecHit3Pos, shootDistance, 1 << 10 | 1 << 12 | 1 << 8);
        RaycastHit2D hit4 = Physics2D.Raycast(transform.position, vecHit4Pos, shootDistance, 1 << 10 | 1 << 12 | 1 << 8);


        //如果放到list的话每次update，add和遍历lst会带来比较明显的性能损耗。所以这里就用单纯的比较来判断了。
        float fDistance = Mathf.Infinity;
        Vector3 vecPoint = new Vector3();
        bool bDrawLine = false;
        if (hit1.collider == targetBox && hit0.distance < fDistance)
        {
            fDistance = hit0.distance;
            vecPoint = hit0.point;
            bDrawLine = true;
        }
        else if (hit1.collider == targetBox && hit1.distance < fDistance )
        {
            fDistance = hit1.distance;
            vecPoint = hit1.point;
            bDrawLine = true;
        }
        else if (hit2.collider == targetBox && hit2.distance < fDistance)
        {
            fDistance = hit2.distance;
            vecPoint = hit2.point;
            bDrawLine = true;
        }
        else if (hit3.collider == targetBox && hit3.distance < fDistance)
        {
            fDistance = hit3.distance;
            vecPoint = hit3.point;
            bDrawLine = true;
        }
        else if (hit4.collider == targetBox && hit4.distance < fDistance)
        {
            fDistance = hit4.distance;
            vecPoint = hit4.point;
            bDrawLine = true;
        }
        if(bDrawLine == true)
        {
            lockedOnObjectLine.SetPosition(0, transform.position);
            lockedOnObjectLine.SetPosition(1, vecPoint);
            lockedOnObjectLine.startWidth = 1f;
        }
        else
        {
            lockedOnObjectLine.startWidth = 0;
        }


        swap.col = null;
        if (hit1.collider == targetBox || hit2.collider == targetBox || hit3.collider == targetBox || hit4.collider == targetBox)
        {
            swap.col = closestObjectToCursor.GetComponent<BoxCollider2D>();
            closestObjectToCursor.GetComponent<Thing>().RegisteDestroyNotify(_unregisteSwapCollide);
            lockedOnObjectLine.startWidth = 5f;
            res = true;
        }
        else
        {
            int a = 0;
        }
        if (swap.col && Vector3.Distance(swap.col.transform.position, transform.position) > shootDistance)
        {
            lockedOnObjectLine.startWidth = 0f;
            res = false;
            swap.col = null;
        }
        return res;
    }

    private void _unregisteSwapCollide()
    {
        swap.col = null;
    }
    private void _resetClosestData()
    {
        closestDistance = Mathf.Infinity;
        closestPlayerDistance = Mathf.Infinity;

        //closestObjectToCursor = null;
        closestObjectToPlayer = null;
        cacheCursorTarget = null;
    }
    // 计算与鼠标和玩家最近的物体
    void HandleObjectDistance() {

        if (toggleSwapTarget) return;

        // 手柄瞄准缓存上一个瞄准的物体
        // 如果距离太远，清掉缓存
        // 如果已瞄准物体死亡，清掉缓存
        // 或者如果玩家移动了瞄准摇杆，清掉缓存
        if ((closestObjectToCursor && closestObjectToCursor.GetComponent<Thing>().dead)
            || !(controlState == ControlWay.isKeyboard) && laserBulletAngle && (closestObjectToCursor && Vector3.Distance(closestObjectToCursor.transform.position, transform.position)> shootDistance)
            || (((player.GetAxis("AimHorizontal") != 0 || player.GetAxis("AimVertical") != 0)) && _canUsingAim()))
        {
            _resetClosestData();
        }
        if( controlState == ControlWay.isKeyboard )
        {
            Vector3 vecMouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(m_vecMouseWorldPos != vecMouseWorldPos)
            {
                m_vecMouseWorldPos = vecMouseWorldPos;
                _resetClosestData();
            }
        }
        
        if (cacheCursorTarget != null && ClickChangeDirectly == false)
        {
            //Vector2 vecDir = (cacheCursorTarget.transform.position - transform.position).normalized;
            //RaycastHit2D _rayCast = Physics2D.Raycast(transform.position, vecDir, shootDistance, TouchLayer);
            if (Hit(cacheCursorTarget) == false)
            {
                cacheCursorTarget = null;
                closestDistance = Mathf.Infinity;
            }
        }

        foreach (var thing in thingList) {

            // 选择离鼠标最近物体
            if (!laserBulletAngle)
            {
                if (thing != null)
                {
                    Vector2 vecMouseWorldPos = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    float distanceToCursor = Vector2.Distance(vecMouseWorldPos, (Vector2)thing.transform.position);
                    float distanceToPlayer = Vector2.Distance((Vector2)transform.position, (Vector2)thing.transform.position);
                    if (ClickChangeDirectly == false)
                    {
                        if (!thing.dead && distanceToCursor < closestDistance && distanceToCursor < cursorSnapThreshold && thing.enabled == true && !thing.hasShield)
                        {
                            if (Hit(thing.gameObject))
                            {
                                closestDistance = distanceToCursor;
                                //closestObjectToCursor = thing.gameObject;
                                cacheCursorTarget = thing.gameObject;
                            }

                        }
                    }

                    if (ClickChangeDirectly == true)
                    {
                        if(distanceToCursor < closestDistance && distanceToPlayer <= DistanceOfPlayerAndThing && !thing.hasShield)
                        {
                            closestDistance = distanceToCursor;
                            cacheCursorTarget = thing.gameObject;
                        }
                    }
                    else
                    {
                        if (!thing.dead && distanceToPlayer < closestPlayerDistance)
                        {
                            closestPlayerDistance = distanceToPlayer;
                            closestObjectToPlayer = thing.gameObject;
                        }
                    }
                }
            }
            // 选择角度最近物体
            else
            {
                if (thing != null && !thing.dead)
                {
                    Vector3 vecMouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    //Debug.Log(vecMouseWorldPos);
                    float angleToCursor = AngleBetween(transform.position, vecMouseWorldPos);

                    float diff = 0.0f;

                    float angleToPlayer = AngleBetween(transform.position, thing.transform.position);
                    //Rewired------------------------------------------------------------------------------
                    if (_canUsingAim() && (player.GetAxis("AimHorizontal") != 0 || player.GetAxis("AimVertical") != 0))
                    {
                        Vector2 dir = new Vector2(player.GetAxis("AimHorizontal"), player.GetAxis("AimVertical")).normalized;
                        angleToCursor = AngleBetween(Vector2.zero, dir);
                        aimAngle = angleToCursor;
                    }
                    else if (!(controlState == ControlWay.isKeyboard) && closestObjectToCursor)
                    {
                        // 如果使用手柄并且右摇杆没有推动，并且已经有一个锁定目标，则跳过搜索
                        continue;
                    }

                    diff = AngleDiff(angleToCursor, angleToPlayer);

                    if (!thing.dead && diff < closestDistance && diff < cursorSnapThreshold && thing.enabled == true && !thing.hasShield)
                    {
                        if (Hit(thing.gameObject) && _canUsingAim())
                        {
                            closestDistance = diff;
                            /*closestObjectToCursor = */cacheCursorTarget = thing.gameObject;
                        }
                    }

                    if (!thing.dead && diff < closestPlayerDistance)
                    {
                        closestPlayerDistance = diff;
                        closestObjectToPlayer = thing.gameObject;
                    }
                }
            }
        }

        if( TempObjectToCursor == cacheCursorTarget )
        {
            m_fTickWaitCursorTime += ( Time.deltaTime / Time.timeScale);
            if(m_fTickWaitCursorTime >= GetWaitCursorTime() && !Input.GetMouseButton(1) && !Input.GetMouseButton(0))
            {
                closestObjectToCursor = cacheCursorTarget;
            }
        }
        else
        {
            //Debug.Log(string.Format("cacheCursorTarget[{0}]", cacheCursorTarget));
            m_fTickWaitCursorTime = 0.0f;
            if(TempObjectToCursor == null && cacheCursorTarget != null && !Input.GetMouseButton(1) && !Input.GetMouseButton(0))
            {
                closestObjectToCursor = cacheCursorTarget;
            }
            TempObjectToCursor = cacheCursorTarget;
        }
        //cacheCursorTarget = null;
        bool bAimingCancel = false;
        if (controlState != ControlWay.isKeyboard)
        {
            float fHorizontal = player.GetAxis("AimHorizontal");
            float fVertical = player.GetAxis("AimVertical");
            Vector2 vec = new Vector2(fHorizontal, fVertical);
            float fDis = vec.magnitude;

            if (fDis < 1)
            {
                closestObjectToCursor = null;
                closestObjectToPlayer = null;
                if( fDis <= 0 )
                {
                    m_bulletTime.ActiveBulletTime(false,BulletTime.BulletTimePriority.BulletTimePriority_Low);
                }
            }
            else
            {
                if(_canUsingAim() == true)
                {
                    bAimingCancel = true;
                    m_bulletTime.ActiveBulletTime(true, BulletTime.BulletTimePriority.BulletTimePriority_Low);
                }
            }

        }
        if ((!prevClosestObjectToCursor && closestObjectToCursor) || prevClosestObjectToCursor != closestObjectToCursor)
        {
            prevClosestObjectToCursor = closestObjectToCursor;
            PlayerControl1.Instance.player.SetVibration(0, landingMotor1Level, landingMotor1duration);
            PlayerControl1.Instance.player.SetVibration(1, landingMotor2Level, landingMotor2duration);
        }
        // 记号圆圈
        if (closestObjectToCursor != null && _canUsingAim() == true) {
            //marker.transform.position = new Vector3(closestObjectToCursor.transform.position.x, closestObjectToCursor.transform.position.y, -1f);
            m_marker.UpdateTarget(closestObjectToCursor.transform);
            FourCornerHit();
            if (locked)
            {
                lockedOnObjectLine.SetPosition(0, transform.position);
                lockedOnObjectLine.SetPosition(1, swap.col.transform.position);
            }
            //m_bulletTime.ActiveBulletTime(true,BulletTime.BulletTimePriority.BulletTimePriority_Low);
        } else {
            CancelMarker(bAimingCancel);
            //m_bulletTime.ActiveBulletTime(false, BulletTime.BulletTimePriority.BulletTimePriority_Low);
        }
        //m_doubleSwap.ProcessDoubleSwap(swap);

    }
    public void CancelMarker(bool bAimingCancel)
    {
        if (bAimingCancel == true)
        {
            swap.col = null;
        }
        //marker.transform.position = new Vector3(-10000f, 0f, 0f);
        m_marker.CloseMarker();
        lockedOnObjectLine.SetPosition(0, Vector3.zero);
        lockedOnObjectLine.SetPosition(1, Vector3.zero);
    }

    private bool _canUsingAim()
    {
        if( controlState == ControlWay.isJoystick )
        {
            if( TouchControl.Instance.CancelLockfunc() == true )
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return true;
        }
    }

    public float GetWaitCursorTime()
    {
        if (controlState == ControlWay.isKeyboard)
        {
            return WaitCursorTimeForKeyboard;
        }
        else
        {
            return WaitCursorTime;
        }
    }
    public PlayerDoubleSwap GetPlayerDoubleSwap()
    {
        return m_doubleSwap;
    }

    void HandlePointer() {
        Vector2 rbNormal = rb.velocity.normalized;
        if (Time.timeScale == 1f || rbNormal == Vector2.zero) {
            pointer.GetComponent<SpriteRenderer>().enabled = false;
            return;
        }
        pointer.GetComponent<SpriteRenderer>().enabled = true;
        float angle = Vector2.SignedAngle(Vector2.right, rbNormal);
        //Debug.Log (angle);
        pointer.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void FixedUpdate() {
        if (rb.velocity != Vector2.zero) rb.gravityScale = 165f;
        if (m_bJumpingWindow == true)
        {


            m_fCurrentKeepJumping += Time.fixedDeltaTime;
            if (m_fCurrentKeepJumping <= JumpAddForceTime)
            {
                float fTimeBase = Time.fixedDeltaTime / Time.timeScale;
                rb.AddForce(transform.up * fTimeBase * jumpForceAir);
            }
            else
            {
                m_bJumpingWindow = false;
            }

        }
        // 左键子弹时间
        //Rewired------------------------------------------------------------
        //if (Input.GetMouseButton(0) || player.GetButton("Switch") || (controlState == ControlWay.isMobile && TouchControl.Instance.aimDrag))
        if (player.GetButton("Switch") && swap.IsSwapCoolDownValid() == true || (controlState == ControlWay.isMobile && TouchControl.Instance.aimDrag))
        {
            currWaitTime += 1;
        }
        //Rewired------------------------------------------------------------
        //if (Input.GetMouseButton(0) || player.GetButton("Switch") || (controlState == ControlWay.isMobile && TouchControl.Instance.aimDrag)) {
        if (player.GetButton("Switch") && swap.IsSwapCoolDownValid() == true || (controlState == ControlWay.isMobile && TouchControl.Instance.aimDrag))
        {
            anim.SetBool("IsCharging", true);

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

        if(m_bDashMove == true)
        {
            m_fCurDashDuration += Time.fixedDeltaTime;
            if( m_fCurDashDuration >= DashingMoveTime)
            {
                m_bDashMove = false;
                m_bJumpRelease = true;
                m_bDashResult = true;
            }
        }
    }

    void HandleLineRenderer() {
        lr.SetPosition(0, transform.position);
        Vector2 mousePosition = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        lr.SetPosition(1, transform.position + (Vector3)mousePosition.normalized * 9999);
    }

    IEnumerator RestoreTimeScale(float duration) {
        yield return new WaitForSeconds(duration);

    }

    void HandleRewind() {

        return;

        if (Rewind.Instance != null) {
            if (Input.GetKey(KeyCode.Space)) {

                Rewind.Instance.isReverting = true;
            } else {
                Rewind.Instance.Record();
                Rewind.Instance.isReverting = false;
            }

            if (!Rewind.Instance.isReverting) { //TODO: if no char or mushroom is moving, don't record

            } else {
                if (Rewind.Instance.states.Count == 0) {
                    return;
                }
                Rewind.Instance.Revert();
            }
        }
    }

    void HandleShootDistanceAndLight() {
        shootDistanceLight.pointLightOuterRadius = shootDistance;
    }

    void IncreaseBulletSpeed() {
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
    void HandleLaserChange() {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!closestObjectToCursor) return;
        FourCornerHit();

        swap.Do();


        StartCoroutine(laserLine());
    }

    void HandleLaserAngle()
    {
        HandleLaserChange();
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
    IEnumerator laserLine() {
        //lr.enabled = true;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position + (closestObjectToCursor.transform.position - transform.position).normalized * shootDistance);
        yield return new WaitForSeconds(0.3f);
        lr.enabled = false;
    }

    //龙王，直接点击直接交换
    void HandleChangeDirectly() {
        if (!closestObjectToCursor) return;
        swap.col = closestObjectToCursor.GetComponent<BoxCollider2D>();
        swap.Do();
    }

    void Shoot() {

        if (ClickChangeDirectly) {
            HandleChangeDirectly();
            return;
        }

        if (laserBullet) {
            HandleLaserChange();
            return;
        }

        if (lockLaserBullet)
        {
            HandleLockLaser();
            return;
        }
        if (laserBulletAngle && m_bDashRequest == false)
        {
            HandleLaserAngle();
            return;
        }

        if (toggleSwapTarget)
        {
            if (!toggleTarget) return;
            swap.Do();
            return;
        }

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        GameObject newBullet = Instantiate(bullet, transform.position + ((Vector3)mouseWorldPos - transform.position).normalized * 30f, Quaternion.Euler(0, 0, -AngleBetween(Vector2.left, ((Vector2)mouseWorldPos - (Vector2)transform.position).normalized)));

        if (isHomingBullet && closestObjectToCursor != null)
        {
            newBullet.GetComponent<Bullet>().SetHomingBullet(closestObjectToCursor.transform, homingBulletRotateSpeed, homingBulletSpeed);
            return;
        }

        //修改Bullet的动画
        if (bulletSpeed == maxBulletSpeed) {
            newBullet.GetComponent<Bullet>().SetBulletType(Bullet.BulletType.fast);
        } else newBullet.GetComponent<Bullet>().SetBulletType(Bullet.BulletType.slow);

        Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();

        if (closestObjectToCursor)
            bulletBody.velocity = ((Vector2)closestObjectToCursor.transform.position - (Vector2)transform.position).normalized * bulletSpeed;
        else
            bulletBody.velocity = (mouseWorldPos - (Vector2)transform.position).normalized * bulletSpeed;

        bulletSpeed = minBulletSpeed;
        chargeFrame = 0;

    }

    // 录视频用 按V直接射出慢子弹
    void HandleShootSlowBullet()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject newBullet = Instantiate(bullet, transform.position + ((Vector3)mouseWorldPos - transform.position).normalized * 30f, Quaternion.Euler(0, 0, -AngleBetween(Vector2.left, ((Vector2)mouseWorldPos - (Vector2)transform.position).normalized)));
            newBullet.GetComponent<Bullet>().SetBulletType(Bullet.BulletType.slow);
            Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();
            bulletBody.velocity = (mouseWorldPos - (Vector2)transform.position).normalized * minBulletSpeed;
        }
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


        //Rewired 手柄震动---------------------------------------------------
        PlayerControl1.Instance.player.SetVibration(0, dieMotor1Level, dieMotor1duration);
        PlayerControl1.Instance.player.SetVibration(1, dieMotor2Level, dieMotor2duration);

        if (invincible) return;
        StartCoroutine(BloodEffect());
        StartCoroutine(OnHit());
        audioSource.PlayOneShot(hitClip, 0.3f);
        if (hp > 1)
        {
            hp -= 1;
            anim.SetTrigger("IsHurt");
            return;
        }
        //StartCoroutine(DelayRestart());

        active = false;
        GetComponent<BoxCollider2D>().enabled = false;
        //spriteRenderer.enabled = false;
        //spriteRenderer.enabled = false;
        //foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
        //{
        //    sr.enabled = false;
        //}
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        PlayerDieImmediately();
        //GetComponent<HeadBodySeparation>().PlayerDead(25000);


        //transform.localScale = Vector3.zero;


    }
    private void _delayAction()
    {

    }
    public IEnumerator DelayRestart()
    {
        if (m_playDieAction != null)
        {
            m_playDieAction.Invoke(this);
        }
        else
        {
            //Debug.Assert(false);
        }
        yield return new WaitForSeconds(PlayerSpawnTime);
        StartCoroutine(DelayLoadScene());
    }

    public IEnumerator DelayLoadScene()
    {
        yield return new WaitForSeconds(0.25f);
        Time.fixedDeltaTime = startDeltaTime;
        Time.timeScale = 1f;
        if (levelTest)
            levelTest.AddDeadNum(1);
        //CheckPointTotalManager.instance.strawberryCount = CheckPointTotalManager.instance.maxStrawberryCount - Strawberry.currentAddNum;
        StrawberryMgr.instance.UpdateStrawberryCoutns( Strawberry.currentAddNum);
        StrawberryMgr.instance.SetStrawBerryText();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public override void Revive() {
        active = true;
        GetComponent<BoxCollider2D>().enabled = true;
        spriteRenderer.enabled = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        foreach (var sr in GetComponentsInChildren<SpriteRenderer>()) {
            sr.enabled = true;
        }
        //transform.localScale = originalScale;
    }

    private void FlipFace(float h) {
        if (h > 0) spriteRenderer.flipX = true;
        else if (h < 0) spriteRenderer.flipX = false;


    }

    //李昊明的数学公式计算*1
    public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, float angle) {
        angle = angle * (Mathf.PI / 180f);
        var rotatedX = Mathf.Cos(angle) * (point.x - pivot.x) - Mathf.Sin(angle) * (point.y - pivot.y) + pivot.x;
        var rotatedY = Mathf.Sin(angle) * (point.x - pivot.x) + Mathf.Cos(angle) * (point.y - pivot.y) + pivot.y;
        return new Vector3(rotatedX, rotatedY, 0);
    }

    //李昊明的数学公式计算*2
    public static float AngleBetween(Vector2 a, Vector2 b) {
        return Mathf.Atan2(b.y - a.y, b.x - a.x);
    }

    private void _logHeight()
    {
        if (canJump == false)
        {
            //Debug.Log(string.Format("Most Height is {0} force is {1}", m_fHeight, m_fTotalForce));
        }
    }

    void HandleJump() {
        if (!isTouchingGround)
            StartCoroutine(JumpTolerence());
        else
        {
            if (m_bJumpingWindow == false && dash.isDashing == false)
            {
                if( m_stateMgr.GetPlayerState() != PlayerStateDefine.PlayerState_Typ.playerState_IdleDash)
                {
                    //if(rb.velocity.y == 0)
                    {
                        _logHeight();
                        canJump = true;
                        m_stateMgr.SetPlayerState(PlayerStateDefine.PlayerState_Typ.playerState_Idle);
                        m_bJumpRelease = false;
                    }
                }
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

    IEnumerator JumpTolerence() {
        int curr = 0;
        while (curr <= coyoteTime) {
            if (isTouchingGround) {

                _logHeight();
                canJump = true;
                //m_stateMgr.SetPlayerState(PlayerStateDefine.PlayerState_Typ.PlayerState_None);
                m_stateMgr.SetPlayerState(PlayerStateDefine.PlayerState_Typ.playerState_Idle);
                m_bJumpRelease = false;
                yield return null;
            }

            yield return new WaitForEndOfFrame();
            curr++;
        }
        canJump = false;
    }

    IEnumerator CacheJump() {
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
        if(CameraShaker.Instance != null)
        {
            CameraShaker.Instance.ShakeOnce(40f, 5f, 0.1f, 0.1f);
        }

        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.color = Color.white;
        invincible = false;

        //Rewired 手柄震动---------------------------------------------------
        PlayerControl1.Instance.player.SetVibration(0, onHitMotor1Level, onHitMotor1duration);
        PlayerControl1.Instance.player.SetVibration(1, onHitMotor2Level, onHitMotor2duration);
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
        while (player.GetButton("Dash") || player.GetButton("Switch") && swap.IsSwapCoolDownValid() == true/*&& swap.delaying*/)
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
        if(swap.col==null)
        {
            isPlayColShadow = false;
            StopAllCoroutines();
        }
        if(swap.col )
        {
            swapColPosition = swap.col.transform.position;
            isPlayColShadow = false;
        }

        yield return new WaitForSeconds(0.1f);
        playerShadow.sprite = spriteRenderer.sprite;
        playerShadow.flipX = spriteRenderer.flipX;
        playerShadow.color = new Color(0, 0, 0, 100 / 255f);

        if (swap.col)
        {
            if (swap.col.transform.position != swapColPosition)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (playerShadow != null)
                    {
                        if (playerShadow == null || swap.col==null)
                        {
                            StopAllCoroutines();
                        }
                        else
                        {
                            SpriteRenderer s = Instantiate(playerShadow, swap.col.transform.position, Quaternion.identity);
                            if (s == null)
                                StopAllCoroutines();
                            s.enabled = true;
                            s.GetComponent<AutoDestroy>().StartDestroy(0.5f + i / 10f);
                            yield return new WaitForSeconds(0.04f);
                        }
                    }
                    
                }

            }
        }
        isPlayColShadow = false;
    }


    //抛物线的显示
    void HandleTrajectoryTest()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (trajectoryOn)
            {
                lr.startColor = new Color(0f, 0f, 0f, 0f);
                lr.endColor = new Color(0f, 0f, 0f, 0f);
            } else
            {
                lr.startColor = trajectoryStartColor;
                lr.endColor = trajectoryEndColor;
            }
            trajectoryOn = !trajectoryOn;
        }
    }

    //键盘切换锁定敌人
    void HandleToggleSwapTarget()
    {
        if (!toggleSwapTarget) return;

        //index = 0;
        swappable = new List<GameObject>();
        foreach (var thing in thingList)
        {
            if (thing != null)
            {
                Vector2 vecMouseWorldPos = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
                float distanceToPlayer = Vector2.Distance((Vector2)transform.position, (Vector2)thing.transform.position);

                if (!thing.dead && thing.enabled == true && !thing.hasShield && distanceToPlayer < shootDistance)
                {
                    swappable.Add(thing.gameObject);
                }
            }
        }
        // 根据x坐标排序
        swappable.Sort((emp1, emp2) => emp1.transform.position.x.CompareTo(emp2.transform.position.x));

        if (swappable.Count == 0) return;

        if (!toggleTarget)
        {
            toggleTarget = swappable[0];
        }

        if (!swappable.Contains(toggleTarget))
        {
            index = 0;
            toggleTarget = swappable[0];
            for (int i = 0; i < swappable.Count; i++)
            {
                if (swappable[i].transform.position.x > toggleTarget.transform.position.x)
                    break;
                toggleTarget = swappable[i];
                index = i;
            }
        }
        else
        {
            index = swappable.IndexOf(toggleTarget);
        }

        //if (Input.GetKeyUp(KeyCode.E)||player.GetButtonDown("LockLeft"))
        //{
        //    if (index == swappable.Count - 1) index = -1;
        //    toggleTarget = swappable[index + 1];
        //    index += 1;
        //}

        //if (Input.GetKeyUp(KeyCode.Q) || player.GetButtonDown("LockRight"))
        //{
        //    if (index == 0) index = swappable.Count;
        //    toggleTarget = swappable[index - 1];
        //    index -= 1;
        //}

        //marker.transform.position = new Vector3(toggleTarget.transform.position.x, toggleTarget.transform.position.y, -1f);
        m_marker.UpdateTarget(toggleTarget.transform);

        if (Hit(toggleTarget))
        {
            swap.col = toggleTarget.GetComponent<BoxCollider2D>();
            lockedOnObjectLine.SetPosition(0, transform.position);
            lockedOnObjectLine.SetPosition(1, toggleTarget.transform.position);
            lockedOnObjectLine.startWidth = 5f;
        }
        else
        {
            lockedOnObjectLine.startWidth = 0f;
            swap.col = null;
        }
    }

    private void _dashStart()
    {
        m_bDashResult = false;
        m_bDashMove = true;
        m_bDashing = true;
        m_fCurDashDuration = 0.0f;
    }
    private void _dashOver()
    {
        //m_bDashing = false;
    }

    public float AngleDiff(float angle1, float angle2)
    {
        if ((angle1 >= 0 && angle2 >= 0) || (angle1 < 0 && angle2 < 0))
        {
            return Mathf.Abs(angle1 - angle2);
        }
        else if (angle1 < 0)
        {
            return Mathf.Min(Mathf.Abs(angle1) + Mathf.Abs(angle2), angle1 + Mathf.PI + Mathf.PI - angle2);
        }
        else
        {
            return Mathf.Min(Mathf.Abs(angle1) + Mathf.Abs(angle2), angle2 + Mathf.PI + Mathf.PI - angle1);

        }
    }
    public void GenerateMoveEffect()
    {
        Instantiate(MoveSmokeEffect, transform.position - Vector3.up * 10, Quaternion.identity);
    }

    public bool IsKeyBoard()
    {
        return controlState == ControlWay.isKeyboard;
    }
    public bool IsJoystick()
    {
        return controlState == ControlWay.isJoystick;
    }
    public bool IsMobile()
    {
        return controlState == ControlWay.isMobile;
    }

    public void PlayerDieImmediately()
    {
        Thing _playerThing = GetComponent<Thing>();
        if(_playerThing != null)
        {
            if (_playerThing.type == Ben.Type.player)

            {
                anim.SetBool("isDead", true);
                //_playerThing.Die();

                StartCoroutine(_playerThing.GetComponent<PlayerControl1>().DelayRestart());
            }
        }
    }

    public bool CanPlaySound()
    {
        return m_enemySound.CanPlaySound();
    }
}