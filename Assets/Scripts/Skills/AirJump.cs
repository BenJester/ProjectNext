using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirJump : Skill
{
    public int maxCharge;
    public int charge;
    public float CoyoteDelay;
    public float CoyoteDuration;
    
    public override void Init()
    {
        charge = maxCharge;
    }

    void FixedUpdate()
    {
        if (playerControl.isTouchingGround)
        {
            charge = maxCharge;
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || playerControl.player.GetButtonDown("Jump"))
        {
            Do();
        }

    }

    public override bool Check()
    {
        return !playerControl.isTouchingGround && charge > 0;
    }

    public override void Do()
    {
        if (!Check())
			return;
        charge -= 1;
        playerBody.velocity = new Vector2(playerBody.velocity.x, playerControl.jumpSpeed);
        StartCoroutine(Coyote());
        GameObject part = Instantiate(playerControl.landingParticle, transform.position - Vector3.up * 10, Quaternion.identity);
        Destroy(part, 2f);
        audioSource.PlayOneShot(clip, 0.35f);
    }

    IEnumerator Coyote()
    {
        yield return new WaitForSeconds(CoyoteDelay);
        float curr = 0f;
        while (curr < CoyoteDuration)
        {
            //playerBody.gravityScale = 75f;
            curr += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        //playerBody.gravityScale = gravity;
    }
}
