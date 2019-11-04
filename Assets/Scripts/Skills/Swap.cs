﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Swap : Skill {

	public bool swapDamageOn;
	public int swapDamage;
	public bool smokeOn;
	public bool damageEffectOn = true;
    
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

    private Vector2 m_vecCacheDrawBoxPos;
    private Vector2 m_vecCacheDrawBoxSize;
    private float m_fCacheDrawBoxAngle;

    private bool m_bDrawBox;

    private Collider2D m_lastTargetCol;
    private bool m_bDoubleSwap;
    public override void Do()
	{
		if (!active || !col || col.GetComponent<Thing> ().dead || !cooldowned)
			return;
		StartCoroutine(DelayedSwap(waitTime));

       // playerControl.SetColShadow();

	}


	public void SetPowerParticle(GameObject powerParticle){
		powerParticle.transform.position=col.transform.position;
		powerParticle.transform.SetParent(col.transform);
		Destroy(powerParticle,0.5f);
	}

    private void _swapThingDestroy()
    {
        m_lastTargetCol = null;
        m_bDoubleSwap = false;
    }

	public void DoSwap ()
    {	
		StartCoroutine (SwapDamageEffect ());

		//屏幕震动	
		CameraShaker.Instance.ShakeOnce(10f,0.1f,0.02f,0.05f);

		ScanEnemies ();
        if(col == null)
        {
            return;
        }
        Collider2D _readySwapCol = col;
        if( m_bDoubleSwap == false )
        {
            Thing _colThing = _readySwapCol.GetComponent<Thing>();
            if(_colThing != null)
            {
                m_lastTargetCol = _readySwapCol;
                _colThing.RegisteDestroyNotify(_swapThingDestroy);
            }
            else
            {
                Debug.Assert(false);
            }
        }
        else
        {
            _readySwapCol = m_lastTargetCol;
            m_bDoubleSwap = false;
            if (m_lastTargetCol == null)
            {
                return;
            }
        }


        Rigidbody2D thingBody = _readySwapCol.gameObject.GetComponent<Rigidbody2D> ();
		Thing _swapThing = _readySwapCol.gameObject.GetComponent<Thing> ();
        _swapThing.ThingSwap();
        Thing _playerThing = player.gameObject.GetComponent<Thing>();
        _playerThing.ThingSwap();
        if (_swapThing.hasShield) return;
		Vector3 posPlayer = player.transform.position;
		Vector3 _posSwapThing = _readySwapCol.transform.position;

        EnergyIndicator.instance.CloseEnergyParticle();
        BoxCollider2D objCol2d = _readySwapCol.GetComponent<BoxCollider2D>();
        //float playerRadiusY = player.GetComponent<BoxCollider2D> ().size.y / 2f;
        float playerRadiusY = player.GetComponent<BoxCollider2D>().bounds.size.y / 2f;
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
			
		}
        else
        {
            _readySwapCol.gameObject.transform.position = new Vector3(posPlayer.x, _playerThing.GetLowerY() + playerRadiusY + heightDiff, posPlayer.z);
            player.transform.position = new Vector3(_posSwapThing.x, _posSwapThing.y - heightDiff, _posSwapThing.z);
            PostEffectManager.instance.Blink (0.03f);
			//print ("Exchange!");
        }
        Smoke();

        //转移粒子：
        EnergyIndicator.instance.TransferEnergyParticle(_readySwapCol.transform);
        EnergyIndicator.instance.RespawnEnergyParticle();

        Vector2 tempV = playerBody.velocity;
        //
		playerBody.velocity = thingBody.velocity;
        //
        playerBody.velocity = new Vector2(playerBody.velocity.x, Mathf.Max(playerBody.velocity.y, 0f));
		thingBody.velocity = tempV;

        cooldowned = false;
        StartCoroutine(StartCooldown());

        audioSource.PlayOneShot(clip, 0.8f);
    }
		
	void ScanEnemies () {
		if (!swapDamageOn)
			return;


		//Collider2D[] cols = Physics2D.OverlapAreaAll ((Vector2) (player.transform.position + scanBoxHeight / 2f * Vector3.up), (Vector2) (col.transform.position - scanBoxHeight / 2f * Vector3.down));
		Vector2 midPoint = (player.transform.position + col.transform.position) / 2f;
		Vector2 size = new Vector2 (Vector2.Distance ((Vector2)player.transform.position, (Vector2)col.transform.position), scanBoxHeight);
		float angle = Vector2.SignedAngle (player.transform.position, col.transform.position);
		//RaycastHit2D[] cols = Physics2D.LinecastAll ((Vector2)player.transform.position, (Vector2)col.transform.position);
		GameObject temp = new GameObject();
		GameObject scan = Instantiate(temp, midPoint, Quaternion.Euler(0f,0f, Vector2.SignedAngle (Vector2.right, (Vector2)player.transform.position - (Vector2)col.transform.position)));
		scan.transform.position = midPoint;
		BoxCollider2D scanBox = scan.AddComponent<BoxCollider2D> ();
		scanBox.isTrigger = true;
		scanBox.size = size;
		//Collider2D[] cols = Physics2D.OverlapBoxAll (midPoint, size, angle);
		Collider2D[] cols = new Collider2D[32];
		int count = Physics2D.OverlapCollider(scanBox, new ContactFilter2D(), cols);
		for (int i = 0; i < count; i ++) {
			if (cols[i] == col)
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
            MathUtil.DrawDebugBox(m_vecCacheDrawBoxPos, m_vecCacheDrawBoxSize, m_fCacheDrawBoxAngle,3);
        }
    }

    void Smoke () {
		if (!smokeOn)
			return;
		Vector3 pos = player.transform.position;
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
			
			yield return new WaitForSeconds (0.1f);
			par3.transform.SetParent(null);
			Destroy(par3,1f);
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
		DoSwap ();
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

    void FixedUpdate()
    {
        if (Input.GetMouseButtonUp(1) && delaying)
        {
            Debug.Log("should not appear first");
            StopAllCoroutines();
            StartCoroutine(CancelDelay());
        }
    }
    public void SetDoubleSwap(bool bDouble)
    {
        m_bDoubleSwap = bDouble;
    }
}