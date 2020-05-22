using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_ChangeSpeed : MonoBehaviour
{
    [Header("Dir为零时原路反弹，否则按Dir方向弹出")]
    public Vector2 dir;
    public float speedFactor;
    public float speed;
    public Animator anim;
    public Vector3 DstPosition;
    AudioSource audioSource;
    public AudioClip bounceClip;


    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        bounceClip = Resources.Load<AudioClip>("Sounds/Mushroom");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {


        if (col.tag == "bullet")
            return;
        if (col.GetComponent<Rigidbody2D>() != null) {
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            
            if (dir != Vector2.zero)
            {
                rb.velocity = dir * speed * speedFactor;
                if (anim != null) anim.CrossFade("Mech_Tanhuang", 0.01f);
            }
            else if (speedFactor != 0)
            {
                rb.velocity = -rb.velocity.normalized * speedFactor* speed;
                if (anim != null) anim.CrossFade("Mech_Tanhuang", 0.01f);
            }



        }
       

        

        

        if (col.gameObject.CompareTag("player"))
        {

            col.GetComponent<AirJump>().charge = col.GetComponent<AirJump>().maxCharge;


            PlayerStateManager _stateMgr = col.gameObject.GetComponent<PlayerStateManager>();
            if (_stateMgr != null)
            {
                _stateMgr.SetPlayerState(PlayerStateDefine.PlayerState_Typ.playerState_ChangingSpeed);
            }
            else
            {
                Debug.Assert(false, string.Format("Player has not playerstatemanager component"));
            }

            if(anim!=null) anim.CrossFade("Mech_Tanhuang", 0.01f);
        }

        if (col.gameObject.CompareTag("player") || col.gameObject.CompareTag("thing"))
            audioSource.PlayOneShot(bounceClip, 0.6f);
    }
}
