using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_FlipByPlayer : MonoBehaviour
{
    private Transform m_transPlayer;
    private bool m_bCurrentRight;
    // Start is called before the first frame update
    void Start()
    {
        if(GlobalVariable.GetPlayer() != null)
        {
            m_transPlayer = GlobalVariable.GetPlayer().transform;
        }
        _processFlipEnemy(true);
    }

    private void FixedUpdate()
    {
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
