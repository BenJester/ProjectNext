using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : Skill
{

    public float wallJumpSpeed;
    void Start()
    {
        playerControl = GetComponent<PlayerControl1>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (playerControl.touchingWallLeft() && !playerControl.isTouchingGround)
            {
                playerBody.velocity = new Vector2(wallJumpSpeed, wallJumpSpeed);
                StartCoroutine(DisableAirControl());
            }
            else if (playerControl.touchingWallRight() && !playerControl.isTouchingGround)
            {
                playerBody.velocity = new Vector2(-wallJumpSpeed, wallJumpSpeed);
                StartCoroutine(DisableAirControl());
            }
        }
    }
    IEnumerator DisableAirControl()
    {
        playerControl.disableAirControl = true;
        yield return new WaitForSeconds(0.1f);
        playerControl.disableAirControl = false;
    }
}
