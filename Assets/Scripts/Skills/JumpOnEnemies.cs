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
        playerBody.velocity = new Vector2(playerBody.velocity.x, playerControl.jumpSpeed);
    }
}
