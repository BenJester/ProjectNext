using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDash : Skill
{
    public float cd;
    float currcd;
    public float speed;
    public float duration;
    public float curr;
    private void Update()
    {
        currcd -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && currcd < 0f)
        {
            StartCoroutine(DoDash());
            currcd = cd;
        }
    }

    IEnumerator DoDash()
    {
        playerControl.gameObject.layer = 18;
        playerControl.canMove = false;
        curr = 0f;
        float g = playerBody.gravityScale;
        playerBody.gravityScale = 0f;
        while (curr < duration)
        {
            curr += Time.deltaTime;
            playerBody.velocity = playerControl.spriteRenderer.flipX ? new Vector2(-speed, 0f) : new Vector2(speed, 0f);
            yield return new WaitForEndOfFrame();
        }
        playerControl.gameObject.layer = 9;
        playerControl.canMove = true;
        playerBody.gravityScale = g;
        playerBody.velocity = playerControl.spriteRenderer.flipX ? new Vector2(-playerControl.speed, 0f) : new Vector2(playerControl.speed, 0f);
    }
}
