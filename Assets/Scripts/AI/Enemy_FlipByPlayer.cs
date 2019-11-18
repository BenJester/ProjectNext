using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_FlipByPlayer : MonoBehaviour
{
    public bool ForceUpdateInFixedUpdate;
    private Transform m_transPlayer;
    private bool m_bCurrentRight;
    private bool m_bValid;
    // Start is called before the first frame update
    void Start()
    {
        if(GlobalVariable.GetPlayer() != null)
        {
            m_transPlayer = GlobalVariable.GetPlayer().transform;
        }
        _processFlipEnemy(true);
        m_bValid = true;
    }
    public void UpdateValid(bool bValid)
    {
        m_bValid = bValid;
    }
    private void FixedUpdate()
    {
        if(ForceUpdateInFixedUpdate==true)
        {
            if(m_bValid == true)
            {
                _processFlipEnemy(false);
            }
        }
    }
    private void _processFlipEnemy(bool bForce)
    {
        bool bRight = false;
        if (m_transPlayer.position.x < transform.position.x)
        {
            bRight = true;
        }
        else
        {
            bRight = false;
        }
        if (bForce == true)
        {
            _flipEnemy(bRight);
            m_bCurrentRight = bRight;
        }
        else
        {
            if (bRight != m_bCurrentRight )
            {
                _flipEnemy(bRight);
                m_bCurrentRight = bRight;
            }
        }
    }
    private void _flipEnemy(bool bRight)
    {
        if (bRight == true)
        {
            transform.localRotation = Quaternion.AngleAxis(0, Vector2.up);
        }
        else
        {
            transform.localRotation = Quaternion.AngleAxis(180, Vector2.up);
        }
    }
    public void ProcessFlip(bool bForce)
    {
        _processFlipEnemy(bForce);
    }
}
