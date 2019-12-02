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
    private float m_fPrevPrevAimMagnitude;
    private bool m_bAimDrag;

    private void Start()
    {
        player = ReInput.players.GetPlayer(0);
    }
    private void Update()
    {
        if(player == null)
        {
            return;
        }
        Vector2 dashDir = player.GetAxis2DRaw("DashAimHorizontal", "DashAimVertical");
        float dashMag = dashDir.magnitude;

        Vector2 aimDir = player.GetAxis2DRaw("AimHorizontal", "AimVertical");
        float aimMag = aimDir.magnitude;

        if (prevMagnitude > 0 && dashMag == 0)
            PlayerControl1.Instance.dash.RequestDash();

        //这里改为，如果瞄准轴被拖动过之后，只要前两桢瞄准的值为0的话，才会执行交换。
        //之前一桢判断的话，如果轴在移动到0，0时也会产生交换。
        if (m_bAimDrag == true)
        {
            if (prevAimMagnitude == 0 && aimMag == 0 && m_fPrevPrevAimMagnitude == 0)
            {
                m_bAimDrag = false;
                PlayerControl1.Instance.swap.Do();
                PlayerControl1.Instance.CancelAimBulletTime();
            }
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
            m_bAimDrag = true;
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
        m_fPrevPrevAimMagnitude = prevAimMagnitude;
        prevAimMagnitude = aimMag;
    }
    
}
