using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOnEnemies : Skill
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Enemy>() != null & collision.collider.transform.position.y < transform.position.y)
            StartCoroutine(LateJump());
    }

    IEnumerator LateJump()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        if (clip != null) {
            GetComponent<AudioSource>().clip = clip;
            GetComponent<AudioSource>().Play();
        }
        
        playerBody.velocity = new Vector2(playerBody.velocity.x, playerControl.jumpSpeed);
    }
}
