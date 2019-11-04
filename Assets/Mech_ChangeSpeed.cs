using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_ChangeSpeed : MonoBehaviour
{
    [Header("Dir为零时原路反弹，否则按Dir方向弹出")]
    public Vector2 dir;
    public float speedFactor;
    public Animator anim;
    public Vector3 DstPosition;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D col)
    {
        

        if (col.tag == "bullet")
            return;
        Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
        float speed = rb.velocity.magnitude;

        

        if (dir != Vector2.zero)
        {
            rb.velocity = dir * speed*speedFactor;
            anim.CrossFade("Mech_Tanhuang", 0.01f);
        }
        else if(speedFactor!=0)
        {
            rb.velocity = -rb.velocity * speedFactor;
            anim.CrossFade("Mech_Tanhuang", 0.01f);
        }

        if (col.gameObject.CompareTag("player"))
        {
            PlayerStateManager _stateMgr = col.gameObject.GetComponent<PlayerStateManager>();
            if (_stateMgr != null)
            {
                _stateMgr.SetPlayerState(PlayerStateDefine.PlayerState_Typ.playerState_ChangingSpeed);
            }
            else
            {
                Debug.Assert(false, string.Format("Player has not playerstatemanager component"));
            }

            anim.CrossFade("Mech_Tanhuang", 0.01f);
        }
    }
}
