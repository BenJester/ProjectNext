using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[System.Serializable]
public class PlayerBoosty 
{
    public float BoostyValue;
    public float MaxBoostySpeed;
    private bool m_bBoosty;
    private Rigidbody2D m_rigidbody;
    private Player m_rPlayer;
    public PlayerBoosty(Rigidbody2D _rigid)
    {
        m_rigidbody = _rigid;
    }

    public void SetBoostyData(Rigidbody2D _rigid, Player _rPlayer )
    {
        m_rigidbody = _rigid;
        m_rPlayer = _rPlayer;
    }
    // Update is called once per frame
    public void Update(float fHorizontal)
    {
        if(m_bBoosty == true)
        {
            //Vector2 vecAdd = m_rigidbody.transform.right * Time.deltaTime * BoostyValue * fHorizontal;
            //m_rigidbody.AddForce(vecAdd);
            float fSpeed = Time.deltaTime * BoostyValue * fHorizontal + m_rigidbody.velocity.x;
            fSpeed = Mathf.Abs(fSpeed) > MaxBoostySpeed ? MaxBoostySpeed * fHorizontal : fSpeed;
            m_rigidbody.velocity = new Vector2(fSpeed, m_rigidbody.velocity.y);
        }
    }
    public bool IsBoosty()
    {
        return m_bBoosty;
    }
    public void BoostyProcess()
    {
        if (m_rPlayer.GetButton("Boosty") == true)
        {
            m_bBoosty = true;
        }
        else
        {
            m_bBoosty = false;
        }
    }
}
