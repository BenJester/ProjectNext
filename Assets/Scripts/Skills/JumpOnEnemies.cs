using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOnEnemies : Skill
{

    public bool canKick = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Enemy>() != null & collision.collider.transform.position.y < transform.position.y) {

            StartCoroutine(LateJump());

            // 
            if (canKick && collision.collider.GetComponent<Thing>().hasShield) {
                collision.collider.GetComponent<Thing>().SetShield(false);
                collision.collider.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }


        }
            
    }

    IEnumerator LateJump()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        GetComponent<AirJump>().charge = GetComponent<AirJump>().maxCharge;
        if (clip != null) {
            GetComponent<AudioSource>().clip = clip;
            GetComponent<AudioSource>().Play();
        }
        
        playerBody.velocity = new Vector2(playerBody.velocity.x, playerControl.jumpSpeed);
    }

    public void KickEnemy() { 
        
    
    }
}
