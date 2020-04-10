using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine.Rendering.PostProcessing;
public class Swap : Skill {

    public bool swapDamageOn;
	public int swapDamage;
	public bool smokeOn;
	[HideInInspector]
    public bool damageEffectOn =false;
    
	public Collider2D col;
    public bool delaying;

    public float dashBeforeSwapTime;

    public GameObject smokeParticle;
	public GameObject damageParticle;
    

	public Vector3 smokeOffset;
	public float scanBoxHeight;
	public bool delay;
	public float waitTime;
	public float reducedTimeScale;
    public float realWaitTime;
    public float curr;

    public float cooldown;
    bool cooldowned = true;


    float playerRadiusY;

    private Vector2 m_vecCacheDrawBoxPos;
    private Vector2 m_vecCacheDrawBoxSize;
    private float m_fCacheDrawBoxAngle;

    private bool m_bDrawBox;
    
    public SwapEffectMovement m_swapEffect;


    [Header("手柄震动")]
    public int motorIndex;
    public float level;
    public float duration;

    private Vector3 m_vecPlayerDst;
    private Vector3 m_vecSwapDst;
    private bool m_bSwaping;
    private bool m_bStayinSwap;
    private float m_fCurrentSwapTime;
    public float m_fSwapTime = 5;
    public float m_fSwapSpeed = 1;
    private Transform m_TransSwap;
    private Vector2 m_cachePlayerVelocity;
    private Vector2 m_cacheBodyVelocity;

    private float m_fStayingTime;

    public PostProcessVolume dashVolume;
    public float StayingTime;
    public Animator SwapAnimator;
    public SpriteRenderer SwapAnimatorSpr;
    public string AnimationParamSwapTrig;
    private PlayerDoubleSwap m_doubleSwap;
    private bool m_bOriginalSwapDamageOn;
    public bool momentumSwap;
    public bool directionSwap;
    public Vector3 startingPoint;
    public float directionSwapThreshold;
    public float swapSpeed;
    public GameObject dashPointer;

    public bool overheadSnap;
    public Rigidbody2D overheadRB;
    public float overheadHeight;

    public delegate void OverheadDelegate();
    public event OverheadDelegate OnOverhead;

    public delegate void DropDelegate();
    public event OverheadDelegate OnDrop;

    public delegate void SwapDelegate();
    public event SwapDelegate OnSwap;

    public float inputCancelDelay;
    public Vector2 keyboardDir;
    public bool canceled;
    public bool triggerItemEvent;
    BulletTime bulletTime;


    LineRenderer lr;
    private void Start()
    {
        m_doubleSwap = GetComponent<PlayerDoubleSwap>();
        m_bOriginalSwapDamageOn = swapDamageOn;
        if( SwapAnimator != null )
        {
            SwapAnimatorSpr = SwapAnimator.GetComponent<SpriteRenderer>();
            SwapAnimatorSpr.enabled = false;
        }
        bulletTime = GetComponent<BulletTime>();
        lr = GetComponent<LineRenderer>();
    }

    public override void Do()
	{
        if (!active || !col || col.GetComponent<Thing>().dead /*|| !cooldowned*/ || canceled)
        {
            return;
        }
        m_doubleSwap.SetDoubleSwapObject(col.GetComponent<Thing>());
        StartCoroutine(DelayedSwap(waitTime));
        if(SwapAnimator!=null)
        {
            SwapAnimator.SetTrigger(AnimationParamSwapTrig);
            SwapAnimatorSpr.enabled = true;
        }

       // playerControl.SetColShadow();

    }
    
    public IEnumerator delayedRecoverCancel()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        canceled = false;
    }
    private void Update()
    {
        if (Input.GetMouseButtonUp(1))
            canceled = true;
        if (Input.GetMouseButtonUp(0))
            StartCoroutine(delayedRecoverCancel());
        if (directionSwap)
        {
            if (Input.GetMouseButtonDown(0))
                startingPoint = Input.mousePosition;
            if (Input.GetMouseButton(0) && (Input.mousePosition - startingPoint).magnitude > directionSwapThreshold)
            {
                //Vector2 dir = (Input.mousePosition - startingPoint).normalized;
                Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
                //Vector2 dir = (playerBody.velocity).normalized;
                dashPointer.SetActive(true);
                dashPointer.transform.position = (Vector2)transform.position + dir * 70f;
                dashPointer.transform.localRotation = Quaternion.Euler(0, 0, -Dash.AngleBetween(Vector2.up, dir));
            }
            else if (Input.GetMouseButtonUp(0) || (Input.GetMouseButton(0) && (Input.mousePosition - startingPoint).magnitude < directionSwapThreshold))
            {
                dashPointer.SetActive(false);
            }
        }
        else if (momentumSwap)
        {
            HandleEightDirInput();
            //Debug.Log(keyboardDir);
            if ((Input.GetMouseButton(0) || playerControl.player.GetAxisRaw("AimHorizontal") != 0 || playerControl.player.GetButton("Switch")) && !canceled)
            {
                Vector2 dir = keyboardDir.normalized;
                //Vector2 dir = (playerBody.velocity).normalized;

                dashPointer.SetActive(true);
                dashPointer.transform.position = (Vector2)transform.position + dir * 70f;
                dashPointer.transform.localRotation = Quaternion.Euler(0, 0, -Dash.AngleBetween(Vector2.up, dir));
            }
            else if (Input.GetMouseButtonUp(0) || playerControl.player.GetAxisRaw("AimHorizontal") == 0)
            {
                dashPointer.SetActive(false);
            }
        }
        DrawTrajectory();
    }

    public void SetPowerParticle(GameObject powerParticle){
		powerParticle.transform.position=col.transform.position;
		powerParticle.transform.SetParent(col.transform);
		Destroy(powerParticle,0.5f);
	}

	public void DoSwap ()
    {
        StartCoroutine (SwapDamageEffect ());
        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        //屏幕震动	
        if (ProCamera2DShake.Instance != null)
        {
            ProCamera2DShake.Instance.Shake("ShakePreset");
        }

        //手柄震动 Rewired------------------------------------------------------------------------------------
        PlayerControl1.Instance.player.SetVibration(motorIndex,level, duration);

        if(col == null)
        {
            return;
        }
        Collider2D _readySwapCol = col;
        if( _readySwapCol != null)
        {
            if(m_swapEffect != null)
            {
                //m_swapEffect.StartMoving(transform.position, _readySwapCol.transform.position);
                m_swapEffect.StartMoving( _readySwapCol.transform.position, transform.position);
            }
        }
        else
        {
        }
        ScanEnemies(_readySwapCol);
        Rigidbody2D thingBody = _readySwapCol.gameObject.GetComponent<Rigidbody2D> ();
        if(thingBody == null)
        {
            Debug.Assert(false);
        }
		Thing _swapThing = _readySwapCol.gameObject.GetComponent<Thing> ();
        if (_swapThing == null)
        {
            Debug.Assert(false);
        }
        _swapThing.ThingSwap();
        Thing _playerThing = player.gameObject.GetComponent<Thing>();
        _playerThing.ThingSwap();
        if (_swapThing.hasShield) return;
		Vector3 posPlayer = player.transform.position;
		Vector3 _posSwapThing = _readySwapCol.transform.position;

        if (overheadRB != null && overheadRB.GetComponent<Thing>() == _swapThing)
        {
            DropOverhead();

        }

        if (EnergyIndicator.instance != null)
        {
            EnergyIndicator.instance.CloseEnergyParticle();
        }
        else
        {
        }
        BoxCollider2D objCol2d = _readySwapCol.GetComponent<BoxCollider2D>();
        //float playerRadiusY = player.GetComponent<BoxCollider2D> ().size.y / 2f;
        playerRadiusY = player.GetComponent<BoxCollider2D>().bounds.size.y / 2f;
        //这里的size.y是原始大小，乘以scale的话只能是相对父亲的大小，但是父亲也缩放的话，就有问题了。所以这里改用bounds.size取世界尺寸
        //float heightDiff = (col.GetComponent<BoxCollider2D> ().size.y * col.transform.localScale.y - playerRadiusY * 2f) / 2f;
        float heightDiff = (_readySwapCol.GetComponent<BoxCollider2D>().bounds.size.y - playerRadiusY * 2f) / 2f;

        if (_swapThing.GetLeftX() < player.transform.position.x && 
            _swapThing.GetRightX() > player.transform.position.x && 
            _swapThing.GetLowerY() > player.transform.position.y && 
            _swapThing.GetLowerY() < player.transform.position.y + playerRadiusY + 10f) {
			
			Vector3 temp = _readySwapCol.gameObject.transform.position;
            _readySwapCol.gameObject.transform.position = new Vector3 (
                player.transform.position.x, 
                player.transform.position.y - playerRadiusY + (_swapThing.GetUpperY() - _swapThing.GetLowerY()) / 2f, 
                player.transform.position.z);

			player.transform.position = new Vector3 (
                temp.x,
                _readySwapCol.gameObject.transform.position.y + playerRadiusY + (_swapThing.GetUpperY() - _swapThing.GetLowerY()) / 2f, 
                player.transform.position.z);

            if (overheadSnap && overheadRB == null)
            {
                //HandleOverhead(_swapThing);
            }
        }
        else
        {
            _readySwapCol.gameObject.transform.position = new Vector3(posPlayer.x, (posPlayer.y + heightDiff + 5f), posPlayer.z);
            player.transform.position = new Vector3(_posSwapThing.x, _posSwapThing.y - heightDiff, _posSwapThing.z);
            #region 交换效果1
            //Vector3 vecDstObj = new Vector3(posPlayer.x, _playerThing.GetLowerY() + playerRadiusY + heightDiff, posPlayer.z);
            //Vector3 vecDstPlayer = new Vector3(_posSwapThing.x, _posSwapThing.y - heightDiff, _posSwapThing.z);
            //dashVolume.enabled = true;
            //m_vecPlayerDst = vecDstPlayer;
            //m_vecSwapDst = vecDstObj;
            //m_bSwaping = true;
            //m_TransSwap = _readySwapCol.gameObject.transform;
            //m_fCurrentSwapTime = 0.0f;
            //player.transform.GetComponent<Rigidbody2D>().isKinematic = true;
            //m_TransSwap.GetComponent<Rigidbody2D>().isKinematic = true;
            #endregion
            //叶梓涛说这个可以暂时去掉了。如果不去掉，交换后的子弹时间因为这个代码关闭，但是没有地方打开，所以就会产生子弹时间没有画面表现。
            //PostEffectManager.instance.Blink (0.03f);
			//print ("Exchange!");
        }
        Smoke();

        //转移粒子：
        //if (EnergyIndicator.instance != null)
        //{
        //    EnergyIndicator.instance.TransferEnergyParticle(_readySwapCol.transform);
        //    EnergyIndicator.instance.RespawnEnergyParticle();
        //}
        //else
        //{
        //}

        Vector2 MomentumPlayer = playerBody.velocity * _playerThing.MomentumMass;
        Vector2 MomentumSwapThing = thingBody.velocity * _swapThing.MomentumMass;
        //
        m_cacheBodyVelocity = playerBody.velocity = thingBody.velocity;
        Vector3 diff = Input.mousePosition - startingPoint;
        if (!momentumSwap)
        {
            if (diff.magnitude > directionSwapThreshold && directionSwap && startingPoint != Vector3.negativeInfinity)
                thingBody.velocity = dir.normalized * swapSpeed;
            else
                thingBody.velocity = new Vector2(0f, 250);
        }
        else
        {
            
            playerBody.velocity = new Vector2(playerBody.velocity.x, Mathf.Max(playerBody.velocity.y, 0f));
            m_cachePlayerVelocity = thingBody.velocity = keyboardDir.normalized * 600f;
            
        }
        //

        Thing _thingInstance = thingBody.GetComponent<Thing>();
        if(_thingInstance != null && _thingInstance.IsSwapRotationByVelocity == true)
        {
            float fAngle = Vector3.Angle(thingBody.velocity, thingBody.transform.forward);

            thingBody.transform.rotation = thingBody.GetComponent<Thing>().GetOriginalQuat();

            thingBody.transform.Rotate(thingBody.transform.forward, fAngle);
        }


        //playerBody.velocity = MomentumSwapThing / _playerThing.MomentumMass;

        cooldowned = false;
        StartCoroutine(StartCooldown());

        audioSource.PlayOneShot(clip, 0.8f);
        startingPoint = Vector3.negativeInfinity;


        //在头上交换会举头顶
        HandleOverhead(_swapThing);

        //会触发Trigger Instance
        if (triggerItemEvent)
            TriggerInstanceEvent(_swapThing);

        // playerBody.velocity = Vector2.zero;
        // playerBody.gravityScale = 0f;
    }

    void HandleEightDirInput()
    {
        float h = (Input.GetKey(KeyCode.A) ? -1f : 0f) + (Input.GetKey(KeyCode.D) ? 1f : 0f);
        float v = (Input.GetKey(KeyCode.S) ? -1f : 0f) + (Input.GetKey(KeyCode.W) ? 1f : 0f);

        if (Input.GetKeyDown(KeyCode.A))
            keyboardDir.x = -1f;
        if (Input.GetKeyDown(KeyCode.D))
            keyboardDir.x = 1f;
        if (Input.GetKeyDown(KeyCode.W))
            keyboardDir.y = 1f;
        if (Input.GetKeyDown(KeyCode.S))
            keyboardDir.y = -1f;
        
        if (Input.GetKeyUp(KeyCode.A))
            StartCoroutine(DelayedCancel(true));
        if (Input.GetKeyUp(KeyCode.D))
            StartCoroutine(DelayedCancel(true));
        if (Input.GetKeyUp(KeyCode.W))
            StartCoroutine(DelayedCancel(false));
        if (Input.GetKeyUp(KeyCode.S))
            StartCoroutine(DelayedCancel(false));

        if (playerControl.controlState == PlayerControl1.ControlWay.isJoystick)
        {
            keyboardDir = new Vector2(playerControl.player.GetAxisRaw("MoveHorizontal"), playerControl.player.GetAxisRaw("MoveVertical"));
        }
        //return new Vector2(h, v).normalized;

    }

    IEnumerator DelayedCancel(bool x)
    {
        
        yield return new WaitForSecondsRealtime(inputCancelDelay);
        if (x)
        {
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                keyboardDir = new Vector2(0f, keyboardDir.y);
            }
        }
            
        else
        {
            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                keyboardDir = new Vector2(keyboardDir.x, 0f);
            }
        }
            
    }

    Vector2 TruncateDirection(Vector2 v)
    {

        int truncatedAngle = ((int)(Angle(v) / 45f)) * 45;
        Vector2 res = Vector2.zero;
        switch (truncatedAngle)
        {
            case 0:
                res = new Vector2(0f, 1f) * v.magnitude;
                break;
            case 45:
                res = new Vector2(1f, 1f).normalized * v.magnitude;
                break;
            case 90:
                res = new Vector2(1f, 0f).normalized * v.magnitude;
                break;
            case 135:
                res = new Vector2(1f, -1f).normalized * v.magnitude;
                break;
            case 180:
                res = new Vector2(0f, -1f).normalized * v.magnitude;
                break;
            case 225:
                res = new Vector2(-1f, -1f).normalized * v.magnitude;
                break;
            case 270:
                res = new Vector2(-1f, 0f).normalized * v.magnitude;
                break;
            case 315:
                res = new Vector2(-1f, 1f).normalized * v.magnitude;
                break;
            default:
                break;
        }
        return res;
    }

    public static float Angle(Vector2 p_vector2)
    {
        if (p_vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
        }
    }

    //Swap 触发Trigger Instance
    void TriggerInstanceEvent(Thing swapThing){
        if(swapThing.GetComponent<TriggerItem_Base>()!=null){
            TriggerItem_Base tb = swapThing.GetComponent<TriggerItem_Base>();
            tb.HandleSwapTrigger();
        }
    }

    private void SwapObject(Transform _trans, Vector3 vecDst)
    {
        _trans.position = Vector3.MoveTowards(_trans.position, vecDst,m_fSwapSpeed);
    }

    void ScanEnemies (Collider2D _swapCol) {
		if (!swapDamageOn)
			return;

		Vector2 midPoint = (player.transform.position + _swapCol.transform.position) / 2f;
        Vector2 size = new Vector2 (Vector2.Distance (player.transform.position, _swapCol.transform.position), scanBoxHeight);
        float angle = Vector2.SignedAngle(Vector2.right, (Vector2)player.transform.position - (Vector2)col.transform.position);

        GameObject temp = new GameObject();
		GameObject scan = Instantiate(temp, midPoint, Quaternion.Euler(0f,0f, Vector2.SignedAngle (Vector2.right, (Vector2)player.transform.position - (Vector2)col.transform.position)));
		scan.transform.position = midPoint;
		BoxCollider2D scanBox = scan.AddComponent<BoxCollider2D> ();
		scanBox.isTrigger = true;
		scanBox.size = size;
		Collider2D[] cols = new Collider2D[32];
		int count = Physics2D.OverlapCollider(scanBox, new ContactFilter2D(), cols);
		for (int i = 0; i < count; i ++) {
			if (cols[i] == _swapCol)
				continue;
			Enemy enemy = cols[i].GetComponent<Enemy> ();
			if (enemy != null) {
				enemy.TakeDamage (swapDamage);
			}
		}
        m_vecCacheDrawBoxPos = midPoint;
        m_vecCacheDrawBoxSize = size;
        m_fCacheDrawBoxAngle = angle;
        m_bDrawBox = true;

        Destroy (temp);
		Destroy (scan);
	}

    private void OnDrawGizmos()
    {
        if (m_bDrawBox == true)
        {
            MathUtil.DrawDebugBox(m_vecCacheDrawBoxPos, m_vecCacheDrawBoxSize, m_fCacheDrawBoxAngle,1);
        }
    }

    void Smoke () {
		if (!smokeOn)
			return;
		Vector3 pos = player.transform.position;
        if (col == null) return;
		Vector3 thingPos = col.transform.position;
		GameObject par1 = Instantiate (smokeParticle, player.transform.position + smokeOffset, Quaternion.identity);
		GameObject par2 = Instantiate (smokeParticle, thingPos + smokeOffset, Quaternion.identity);
		Destroy (par1, 1f);
		Destroy (par2, 1f);
	}

	//刀光，加入伤害的粒子效果
	IEnumerator SwapDamageEffect () {
		if (damageEffectOn) {
			GameObject par3 =  Instantiate(damageParticle,transform.position,Quaternion.identity);
            SwapEffectMovement _movement = par3.GetComponent<SwapEffectMovement>();
            if(_movement != null)
            {
                _movement.PlayerTrans = transform;
                m_swapEffect = _movement.GetComponent<SwapEffectMovement>();
            }
            damageEffectOn = false;

        } else {
			yield return null;
		}

	}
	IEnumerator DelayedSwap (float waitTime) {
        
        if (delay) {
            delaying = true;
			Time.timeScale = Mathf.Min(Time.timeScale, reducedTimeScale);
			playerControl.targetTimeScale = Time.timeScale;
            Time.fixedDeltaTime = reducedTimeScale * playerControl.startDeltaTime;
            playerControl.targetDeltaTime = reducedTimeScale;
            realWaitTime = waitTime * Time.timeScale;
            curr = 0f;
            while (curr < realWaitTime)
            {
                if (Input.GetMouseButtonUp(1))
                    break;
                curr += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            Time.fixedDeltaTime = playerControl.startDeltaTime;
            playerControl.targetDeltaTime = playerControl.startDeltaTime;
            //yield return new WaitForSeconds (waitTime * (Time.timeScale == reducedTimeScale ? reducedTimeScale : 1f));
            Time.timeScale = 1f;
			playerControl.targetTimeScale = 1f;
		}
        delaying = false;
        //DoSwap ();
        //StartCoroutine(DoDash());
        //StartCoroutine(Poker());
        //StartCoroutine(PokerShoot());
        StartCoroutine(PokerDash());
        
    }

    IEnumerator CancelDelay()
    {
        delaying = false;
        yield return new WaitForSeconds(dashBeforeSwapTime);
        Time.fixedDeltaTime = playerControl.startDeltaTime;
        playerControl.targetDeltaTime = playerControl.startDeltaTime;
        //yield return new WaitForSeconds (waitTime * (Time.timeScale == reducedTimeScale ? reducedTimeScale : 1f));
        Time.timeScale = 1f;
        playerControl.targetTimeScale = 1f;
        DoSwap();
    }

    IEnumerator StartCooldown()
    {
        cooldowned = false;
        yield return new WaitForSecondsRealtime(cooldown);
        cooldowned = true;
    }
    public bool IsSwapCoolDownValid()
    {
        return cooldowned;
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonUp(1) && delaying)
        {
            Debug.Log("should not appear first");
            StopAllCoroutines();
            StartCoroutine(CancelDelay());
        }

        if (playerControl.isTouchingGround)
        {
            consecutiveThrowCount = 0;
        }
        //if(m_bSwaping == true)
        //{
        //    m_fCurrentSwapTime += Time.fixedDeltaTime;
        //    SwapObject(m_TransSwap, m_vecSwapDst);
        //    SwapObject(player.transform, m_vecPlayerDst);
        //    //if ( m_fCurrentSwapTime >= m_fSwapTime )
        //    //{
        //    //    _SwitchToStaying();
        //    //}
        //    if (m_TransSwap.position.Equals(m_vecSwapDst) == true)
        //    {
        //        _SwitchToStaying();
        //        dashVolume.enabled = false;
        //    }
        //}
        //if(m_bStayinSwap == true )
        //{
        //    m_fStayingTime += Time.fixedDeltaTime;
        //    if(m_fStayingTime >= StayingTime )
        //    {
        //        m_bStayinSwap = false;
        //        _resetVelocity();
        //        EnergyIndicator.instance.RespawnEnergyParticle();
        //    }
        //}
    }

    private void _SwitchToStaying()
    {
        m_bSwaping = false;
        m_bStayinSwap = true;
        m_fStayingTime = 0.0f;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        m_TransSwap.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
    private void _resetVelocity()
    {
        player.GetComponent<Rigidbody2D>().isKinematic = false;
        player.GetComponent<Rigidbody2D>().velocity = m_cacheBodyVelocity;

        m_TransSwap.GetComponent<Rigidbody2D>().isKinematic = false;
        m_TransSwap.GetComponent<Rigidbody2D>().velocity = m_cachePlayerVelocity;
    }

    public void ResetSwapDamageOn()
    {
        swapDamageOn = m_bOriginalSwapDamageOn ;
    }

    void HandleOverhead(Thing thing)
    {
        if (overheadRB != null || !overheadSnap) return;
        if (thing.GetLeftX() < player.transform.position.x &&
            thing.GetRightX() > player.transform.position.x &&
            thing.GetLowerY() > player.transform.position.y &&
            thing.GetLowerY() < player.transform.position.y + playerRadiusY + 60f)
        {
            
            Rigidbody2D rb = thing.GetComponent<Rigidbody2D>();
            overheadRB = rb;
            rb.transform.position = playerControl.transform.position + new Vector3(0f, rb.GetComponent<BoxCollider2D>().bounds.size.y / 2f - rb.GetComponent<BoxCollider2D>().offset.y + 30f, 0f);

            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.transform.parent = playerControl.transform;
            OnOverhead?.Invoke();
        }
            
    }

    public void DropOverhead()
    {
        if (overheadRB != null)
        {
            overheadRB.transform.parent = null;
            overheadRB.bodyType = RigidbodyType2D.Dynamic;
            overheadRB = null;
            OnDrop?.Invoke();
            playerControl.overhead.SwitchState(OverheadState.None);
        }
    }

    public float dashSpeed;
    public float dashDur;

    bool busy;
    public Sprite pokerSprite;

    public float pokerTransitionDur;
    public Sprite turnPokerSprite;


    public float dashReducedTimeScale;



    IEnumerator PokerDash()
    {
        if (busy) yield break;
        busy = true;
        playerControl.invincible = true;
        OnSwap?.Invoke();

        Smoke();
        float timer = Time.realtimeSinceStartup;

        bulletTime.ActiveBulletTime(true, BulletTime.BulletTimePriority.BulletTimePriority_High);


        Collider2D target = col;
        Sprite targetSprite = null;
        Sprite playerSprite = playerControl.spriteRenderer.sprite;
        if (target.GetComponent<SpriteRenderer>() != null)
            targetSprite = target.GetComponent<SpriteRenderer>().sprite;
        else
            targetSprite = target.GetComponent<Thing>().sr.sprite;
        //target.GetComponent<SpriteRenderer>().sprite = pokerSprite;
        //playerControl.spriteRenderer.sprite = pokerSprite;
        player.layer = 18;
        //playerControl.box.enabled = false;
        playerControl.disableAirControl = true;
        bool playerFaceRight = playerControl.spriteRenderer.flipX;

        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        Vector3 diff = Input.mousePosition - startingPoint;

        

        Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
        Vector2 targetV = targetRb.velocity;
        BoxCollider2D targetBox = target.GetComponent<BoxCollider2D>();
        Thing targetThing = target.GetComponent<Thing>();
        if (target.GetComponent<Enemy>() != null)
            target.GetComponent<Enemy>().hpText.SetActive(false);
        bool targetIsTrigger = targetBox.isTrigger;




        Vector3 prevPos = new Vector3(transform.position.x,
                                         playerControl.isTouchingGround ? player.GetComponent<Thing>().GetLowerY() + targetBox.size.y / 2f + 2f : transform.position.y,
                                         transform.position.z);

        Vector3 prevColPos = new Vector3(target.transform.position.x, 
                                         targetThing.isTouchingGround ? targetThing.GetLowerY() + playerControl.box.size.y / 2f: target.transform.position.y, 
                                         target.transform.position.z);



        targetBox.enabled = false;
        audioSource.PlayOneShot(clip, 0.8f);

        //float speed = Vector3.Distance(prevPos, prevColPos) / dashDur;
        targetThing.swapping = true;
        //target.transform.position = prevPos;
        //player.transform.position = prevPos;
        playerControl.spriteRenderer.GetComponent<Animator>().enabled = false;
        if (target.GetComponent<Animator>() != null)
            target.GetComponent<Animator>().enabled = false;
        Vector3 playerScale = transform.localScale;
        Vector3 targetScale = target.transform.localScale;
        SpriteRenderer targetSr = target.GetComponent<SpriteRenderer>();
        if (targetSr == null)
            targetSr = target.GetComponent<Thing>().sr;
        float playerGravity = playerControl.rb.gravityScale;
        float targetGravity = targetRb.gravityScale;
        playerControl.rb.gravityScale = 0f;
        targetRb.gravityScale = 0f;
        float speed = Vector3.Distance(prevColPos, prevPos) / pokerTransitionDur / 3f;
        float curr = 0f;
        //playerControl.spriteRenderer.sprite = turnPokerSprite;
        while (curr < pokerTransitionDur)
        {
            curr += Time.deltaTime;

            playerControl.rb.velocity = (prevColPos - prevPos).normalized * speed;
            if (targetRb != null)
                targetRb.velocity = Vector2.zero;
            //transform.localScale = new Vector3((pokerTransitionDur - curr) / pokerTransitionDur * playerScale.x, playerScale.y, playerScale.z);
            target.transform.localScale = new Vector3((pokerTransitionDur - curr) / pokerTransitionDur * targetScale.x, targetScale.y, targetScale.z);
            yield return new WaitForEndOfFrame();
            ShadowPool.instance.GetFromPool();

        }
        curr = curr - pokerTransitionDur;
        target.transform.position = prevPos;
        while (curr < pokerTransitionDur)
        {
            curr += Time.deltaTime;
            //playerControl.spriteRenderer.sprite = turnPokerSprite;
            targetSr.sprite = turnPokerSprite;
            playerControl.rb.velocity = (prevColPos - prevPos).normalized * speed;
            if (targetRb != null)
                targetRb.velocity = Vector2.zero;
            transform.localScale = new Vector3(curr / pokerTransitionDur, playerScale.y, playerScale.z);
            target.transform.localScale = new Vector3(curr / pokerTransitionDur, targetScale.y, targetScale.z);
            yield return new WaitForEndOfFrame();
            ShadowPool.instance.GetFromPool();

        }
        curr = curr - pokerTransitionDur;
        while (curr < pokerTransitionDur)
        {
            curr += Time.deltaTime;
            //playerControl.spriteRenderer.sprite = turnPokerSprite;
            targetSr.sprite = turnPokerSprite;
            playerControl.rb.velocity = (prevColPos - prevPos).normalized * speed;
            if (targetRb != null)
                targetRb.velocity = Vector2.zero;
            transform.localScale = new Vector3((pokerTransitionDur - curr) / pokerTransitionDur, playerScale.y, playerScale.z);
            target.transform.localScale = new Vector3((pokerTransitionDur - curr) / pokerTransitionDur, targetScale.y, targetScale.z);
            yield return new WaitForEndOfFrame();
            ShadowPool.instance.GetFromPool();

        }
        curr = curr - pokerTransitionDur;
        playerControl.transform.position = prevColPos;
        target.transform.position = prevPos;
        while (curr < pokerTransitionDur)
        {
            curr += Time.deltaTime;
            playerControl.spriteRenderer.sprite = playerSprite;
            targetSr.sprite = targetSprite;
            playerControl.rb.velocity = Vector2.zero;
            if (targetRb != null)
                targetRb.velocity = Vector2.zero;
            transform.localScale = new Vector3(curr / pokerTransitionDur * playerScale.x, playerScale.y, playerScale.z);
            target.transform.localScale = new Vector3(curr / pokerTransitionDur * targetScale.x, targetScale.y, targetScale.z);
            yield return new WaitForEndOfFrame();
            ShadowPool.instance.GetFromPool();

        }
        playerControl.spriteRenderer.GetComponent<Animator>().enabled = true;
        if (target.GetComponent<Animator>() != null)
            target.GetComponent<Animator>().enabled = true;
        playerControl.disableAirControl = false;
        if (!playerControl.touchingFloor())
            playerControl.rb.velocity = new Vector2(0f, 250f);
        Smoke();
        player.layer = 9;
        targetThing.swapping = false;
        playerControl.spriteRenderer.sprite = playerSprite;
        player.transform.localScale = playerScale;
        target.transform.localScale = targetScale;
        playerControl.rb.gravityScale = playerGravity;
        targetRb.gravityScale = targetGravity;
        if (targetRb != null)
        {
            targetSr.sprite = targetSprite;
            //targetBox.isTrigger = targetIsTrigger;
            if (diff.magnitude > directionSwapThreshold && directionSwap && startingPoint != Vector3.negativeInfinity)
            {
                StartCoroutine(targetThing.CancelBeingThrown(0.65f));
                if (targetThing.GetComponent<EnemyBullet_Transable_Forward>() == null)     
                    targetRb.velocity = dir.normalized * swapSpeed;
                else
                    targetRb.velocity = dir.normalized * targetV.magnitude;
                if (prevCol == col && Mathf.Abs(targetRb.gravityScale) >= 10f)
                {
                    consecutiveThrowCount += 1;
                    if (consecutiveThrowCount >= allowedConsecutiveThrowCount)
                        StartCoroutine(CancelDirectionThrow());
                }
                else
                {
                    consecutiveThrowCount = 0;
                }
                prevCol = col;
                
            }

            else if (targetThing.GetComponent<EnemyBullet_Transable_Forward>() == null)
                targetRb.velocity = Vector3.zero;
            else
                targetRb.velocity = targetV;
            target.transform.position = prevPos;
            targetBox.enabled = true;
            if (target.GetComponent<Enemy>() != null)
            {
                //target.GetComponent<Enemy>().faceRight = !playerFaceRight;
                target.GetComponent<Enemy>().hpText.SetActive(true);
            }

            targetThing.swapTriggerMethod?.Invoke();
            
        }
        playerControl.spriteRenderer.flipX = playerFaceRight;
        playerControl.box.enabled = true;
        busy = false;
        playerControl.invincible = false;
        bulletTime.ActiveBulletTime(false, BulletTime.BulletTimePriority.BulletTimePriority_High);

        
    }

    Collider2D prevCol;
    int consecutiveThrowCount;
    public int allowedConsecutiveThrowCount;

    IEnumerator CancelDirectionThrow()
    {
        directionSwap = false;
        yield return new WaitForSeconds(1f);
        consecutiveThrowCount = 0;
        directionSwap = true;
    }
    public float TrajectoryTimeStep;
    public int TrajectoryStepCounts;
    void DrawTrajectory()
    {
        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        Vector3 diff = Input.mousePosition - startingPoint;
        if (!(col != null && diff.magnitude > directionSwapThreshold && directionSwap && startingPoint != Vector3.negativeInfinity && Input.GetMouseButton(0)))
        {
            lr.enabled = false;
            return;
        }
        lr.enabled = true;
        GameObject target = col.gameObject;
        lr.SetPositions(PlotTrajectory(target, transform.position, dir * swapSpeed, TrajectoryTimeStep, TrajectoryStepCounts));
    }

    public Vector3 PlotTrajectoryAtTime(GameObject target, Vector3 start, Vector3 startVelocity, float time)
    {
        return start + startVelocity * time + Physics.gravity * target.GetComponent<Rigidbody2D>().gravityScale * time * time * 0.5f;
    }

    public Vector3[] PlotTrajectory(GameObject target, Vector3 start, Vector3 startVelocity, float fTimeStep, int stepCounts)
    {
        float maxTime = fTimeStep * stepCounts;
        Vector3[] results = new Vector3[stepCounts];
        Vector3 prev = start;
        for (int i = 1; ; i++)
        {
            float t = fTimeStep * i;
            if (t >= maxTime) break;
            Vector3 pos = PlotTrajectoryAtTime(target, start, startVelocity, t);
            if (Physics.Linecast(prev, pos)) break;
            prev = pos;
            results[i - 1] = pos;
        }
        Debug.Log(results.Length);
        return results;
    }
}