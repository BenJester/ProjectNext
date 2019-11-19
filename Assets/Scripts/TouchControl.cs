using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public enum JoystickState
{

}

public class TouchControl : MonoBehaviour
{
    public static TouchControl Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    public float prevMagnitude;
    public float prevAimMagnitude;

    public float dragStartMag;

    Player player;
    public Vector2 finalDashDir;
    public Vector2 finalAimDir;
    public bool dashDrag;
    public bool aimDrag;

    private void Start()
    {
        player = ReInput.players.GetPlayer(0);
    }
    private void Update()
    {
        Vector2 dashDir = player.GetAxis2DRaw("DashAimHorizontal", "DashAimVertical");
        float dashMag = dashDir.magnitude;

        Vector2 aimDir = player.GetAxis2DRaw("AimHorizontal", "AimVertical");
        float aimMag = aimDir.magnitude;

        if (prevMagnitude > 0 && dashMag == 0)
            PlayerControl1.Instance.dash.RequestDash();

        if (prevAimMagnitude > 0 && aimMag == 0)
        {
            PlayerControl1.Instance.swap.Do();
        }
            

        if (dashMag >= dragStartMag)
        {
            finalDashDir = dashDir;
            dashDrag = true;
        }
        else
        {
            finalDashDir = player.GetAxis2DRaw("MoveHorizontal", "MoveAimVertical");
            dashDrag = false;
        }

        if (aimMag >= dragStartMag)
        {
            finalAimDir = aimDir;
            aimDrag = true;
        }
        else
        {
            aimDrag = false;
        }

        if (player.GetButtonUp("QuickDash"))
        {
            PlayerControl1.Instance.dash.RequestDash();
        }

        if (player.GetButtonUp("QuickSwitch"))
        {
            PlayerControl1.Instance.swap.Do();
        }

        prevMagnitude = dashMag;
        prevAimMagnitude = aimMag;
    }
    
}
