using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class ThrowTutorial : MonoBehaviour
{
    PlayerControl1 playerControl;

    public bool active;
    public Text text;
    public Text textEng;
    public float bulletTimeScale = 0.1f;

    public string jump;
    public string holdRightClick;
    public string Release;
    public string LeftClick;

    public string jumpEng;
    public string holdRightClickEng;
    public string ReleaseEng;
    public string LeftClickEng;

    public int state; // 0 = 未起跳, 1 = 起跳, 2 = 冲刺按下, 3 = 冲刺松开 4 = 交换
    public bool bulletTime = true;
    void Start()
    {
        playerControl = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerControl1>();
    }
    
    void SetTimeScale(float scale)
    {
        if (!bulletTime) return;
        Time.timeScale = scale;
        playerControl.targetTimeScale = scale;
        Time.fixedDeltaTime = playerControl.startDeltaTime * scale;
        playerControl.targetDeltaTime = playerControl.startDeltaTime * scale;
    }

    void Update()
    {
        if (!active) return;
        if (playerControl.isTouchingGround)
        {
            state = 0;
            text.text = jump;
            textEng.text = jumpEng;
            SetTimeScale(0.1f);
        }
        else if (!playerControl.dash.isDashing && playerControl.dash.charge > 0 && playerControl.dash.currWaitTime < playerControl.dash.waitTime)
        {
            state = 1;
            text.text = holdRightClick;
            textEng.text = holdRightClickEng;
            SetTimeScale(0.1f);
        }
        else if (!playerControl.dash.isDashing && Input.GetMouseButton(1))
        {
            state = 2;
            text.text = Release;
            textEng.text = ReleaseEng;
            SetTimeScale(0.04f);
        }
        else
        {
            state = 3;
            text.text = LeftClick;
            textEng.text = LeftClickEng;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            active = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            active = false;
            playerControl.targetTimeScale = 1f;
            playerControl.targetDeltaTime = playerControl.startDeltaTime;
        }
    }
}
