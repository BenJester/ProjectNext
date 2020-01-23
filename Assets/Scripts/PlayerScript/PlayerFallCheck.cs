using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallCheck : MonoBehaviour
{
    [Tooltip("待检测masklayer")]
    public LayerMask CheckMask;
    [Tooltip("检测距离")]
    public float CheckDistance;
    [Tooltip("检测高度")]
    public float CheckFallDistance;
    public Animator Anim;
    public PlayerStateManager PlayerState;
    public SpriteRenderer spriteRenderer;
    private Rigidbody2D m_rigidbody;
    private bool m_bTrigger;
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bTrigger == false)
        {
            if(PlayerState.GetPlayerState() == PlayerStateDefine.PlayerState_Typ.playerState_Idle)
            {
                if (!(m_rigidbody.velocity.x > 0 || m_rigidbody.velocity.x < 0 || m_rigidbody.velocity.y < 0 || m_rigidbody.velocity.y > 0))
                {
                    bool bRes = false;
                    RaycastHit2D[] lstHit = Physics2D.RaycastAll(transform.position + transform.right * _getDiffTime() * CheckDistance, -transform.up, CheckFallDistance, CheckMask);
                    if (lstHit.Length > 0)
                    {
                        bRes = true;
                    }
                    if (bRes == false)
                    {
                        Anim.SetTrigger("IsFall");
                        m_bTrigger = true;
                    }
                }
            }
        }
        else
        {
            if ((m_rigidbody.velocity.x > 0 || m_rigidbody.velocity.x < 0))
            {
                m_bTrigger = false;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + transform.right * _getDiffTime() * CheckDistance, transform.position + transform.right * _getDiffTime() * CheckDistance - transform.up * CheckFallDistance);
    }

    private float _getDiffTime()
    {
        float fDiff = 1;
        if (spriteRenderer.flipX == true)
        {
            fDiff = -1;
        }
        else
        {
            fDiff = 1;
        }
        return fDiff;
    }
}
