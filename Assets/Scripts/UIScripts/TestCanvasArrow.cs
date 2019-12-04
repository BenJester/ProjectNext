using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class TestCanvasArrow : MonoBehaviour
{
    public Player player;

    public float Distance;

    public RectTransform transTarget;
    private Vector2 m_vecOriginal;
    private RectTransform m_selfRect;
    private bool m_bInit;
    // Start is called before the first frame update
    void Start()
    {
        m_selfRect = transTarget.GetComponent<RectTransform>();
        m_vecOriginal = m_selfRect.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bInit == false)
        {
            if (PlayerControl1.Instance.player != null)
            {
                player = PlayerControl1.Instance.player;
                m_bInit = true;
            }
        }
        if (player != null)
        {
            float fAimHorizontal = player.GetAxis("AimHorizontal");
            float fAimVertical = player.GetAxis("AimVertical");
            if(fAimVertical<0)
            {
                int a = 0;
            }

            if(fAimHorizontal > 0)
            {
                int a = 0;
            }
            m_selfRect.position = new Vector3(m_vecOriginal.x + Distance * fAimHorizontal, m_vecOriginal.y + Distance * fAimVertical, m_selfRect.position.z);
        }
    }
}
