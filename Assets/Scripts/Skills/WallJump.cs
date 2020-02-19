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
                StartCoroutine(DisableAirControl(true));
                
            }
            else if (playerControl.touchingWallRight() && !playerControl.isTouchingGround)
            {
                StartCoroutine(DisableAirControl(false));
                
            }
        }
    }
    IEnumerator DisableAirControl(bool right)
    {
        playerControl.disableAirControl = true;
        yield return new WaitForEndOfFrame();
        playerBody.velocity = new Vector2(right ? wallJumpSpeed : -wallJumpSpeed, wallJumpSpeed);
        yield return new WaitForSeconds(0.14f);
        playerControl.disableAirControl = false;
    }
}
