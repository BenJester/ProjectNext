using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OverheadState
{
    None, Ironfoot
}

public class Overhead : Skill
{
    float originalGravity;
    bool switched;
    public OverheadState state;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SwitchState(OverheadState newState)
    {
        state = newState;
    }

    void Update()
    {
        switch (state)
        {
            case OverheadState.None:
                break;
            case OverheadState.Ironfoot:
                UpdateIronfoot();
                break;
            default:
                break;
        }
    }
    
    void UpdateIronfoot()
    {
        if (playerBody.velocity.y < 0f)
        {
            //originalGravity = playerBody.gravityScale;
            //playerBody.gravityScale = 0;
            playerBody.velocity = new Vector2(playerBody.velocity.x, -100f);
            //switched = true;
        }
        else
        {
            //playerBody.gravityScale = originalGravity;
        }
    }
}
