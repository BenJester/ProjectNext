using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirJump : Skill
{
    public int maxCharge;
    int charge;
    public float CoyoteDelay;
    public float CoyoteDuration;
    
    public override void Init()
    {
        charge = maxCharge;
    }

    void Update()
    {
        if (playerControl.isTouchingGround)
        {
            charge = maxCharge;
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            Do();
        }

    }

    public override bool Check()
    {
        return !playerControl.canJump && charge > 0;
    }

    public override void Do()
    {
        if (!Check())
			return;
        charge -= 1;
        playerBody.velocity = new Vector2(playerBody.velocity.x, playerControl.jumpSpeed);
        StartCoroutine(Coyote());
    }

    IEnumerator Coyote()
    {
        yield return new WaitForSeconds(CoyoteDelay);
        float curr = 0f;
        while (curr < CoyoteDuration)
        {
            playerBody.gravityScale = 75f;
            curr += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        playerBody.gravityScale = gravity;
    }
}
