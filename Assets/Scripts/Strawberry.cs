﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strawberry : MonoBehaviour
{
    public bool active;
    public float animDuration;
    public Vector3 translation;
    public float rotationSpeed;
    public float StrawBerryFolloingTime;

    public float FollowingSpeed;

    SpriteRenderer sr;
    CheckPointTotalManager worldManager;

    private bool m_bFollingPlayer;
    private PlayerControl1 m_playerCtrl;
    private float m_fCurFollingTime;
    private Vector3 m_vecOriginalPos;
    private void Start()
    {
        if(StrawBerryFolloingTime == 0.0f)
        {
            StrawBerryFolloingTime = 3.0f;
        }
        m_vecOriginalPos = transform.position;
        sr = GetComponent<SpriteRenderer>();
        worldManager = GameObject.FindGameObjectWithTag("WorldManager").GetComponent<CheckPointTotalManager>();
        worldManager.maxStrawberryCount += 1;
        m_playerCtrl = GlobalVariable.GetPlayer();
        GlobalVariable.RegisteSetPlayerEvent(_setPlayer);

        if (m_playerCtrl != null)
        {
            m_playerCtrl.RegisteDieAction(_playerDie);
        }
        else
        {
            Debug.Assert(false);
        }
        if(FollowingSpeed == 0.0f)
        {
            FollowingSpeed = 3.0f;
        }
        
    }
    private void _setPlayer(PlayerControl1 _player)
    {
        m_playerCtrl = _player;
        m_playerCtrl.RegisteDieAction(_playerDie);
    }
    private void OnDestroy()
    {
        GlobalVariable.UnregisteSetPlayerEvent(_setPlayer);
    }
    private void FixedUpdate()
    {
        if( m_bFollingPlayer == true )
        {
            Vector3 vecDst = Vector3.MoveTowards(transform.position, m_playerCtrl.transform.position, FollowingSpeed);
            transform.position = new Vector3(vecDst.x, vecDst.y, transform.position.z);
            m_fCurFollingTime += Time.fixedDeltaTime;
            if(m_fCurFollingTime >= StrawBerryFolloingTime)
            {
                m_bFollingPlayer = false;
                StartCoroutine(Anim(animDuration));
                worldManager.strawberryCount += 1;
                worldManager.SetStrawBerryText();
                active = false;
            }
        }
    }

    private void _playerDie(PlayerControl1 _ctrl)
    {
        m_bFollingPlayer = false;
        transform.position = m_vecOriginalPos;
        GetComponent<BoxCollider2D>().enabled = true;
        active = true;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {

        if (col.CompareTag("player") && active)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            m_bFollingPlayer = true;
            m_fCurFollingTime = 0.0f;

        }
    }


    IEnumerator Anim(float duration)
    {
        float curr = 0f;
        float originalAlpha = sr.color.a;
        while (curr <= 1f)
        {
            sr.color = new Color(1f, 1f, 1f, (1 - curr) * originalAlpha);
            transform.Translate(Time.deltaTime / duration * translation);
            transform.Rotate(0f, rotationSpeed, 0f);
            curr = Mathf.Clamp01(curr + Time.deltaTime / duration);
            yield return new WaitForEndOfFrame();
        }
    }
}
