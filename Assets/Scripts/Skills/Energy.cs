using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Energy : Skill
{
    public float maxEnergy;
    public float energy;
    public float restoreDelay;
    float touchFloorTimer;
    public float bulletTimeSpendDelay;
    public float bulletTimeTimer;
    public float restoreSpeed;
    public float swapCost;
    public float bulletTimeCost;
    public Image hpBar;
    public Image hpBg;
    public Image lossHPbar;
    public Image fatigue;
    public bool freeSwap = true;
    public float lossHPAnimDelay;
    public float lossHPAnimDuration;
    float currDelayTimer;
    bool startTimer;
    Color fatigueColor;
    public Text leftSwapNumText;

    bool init;
    public static Energy Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        fatigueColor = fatigue.color;
    }
    public void Restore()
    {
        touchFloorTimer += Time.deltaTime;
        if (touchFloorTimer > restoreDelay && PlayerControl1.Instance.rb.gravityScale != 0f)
        {
            energy = Mathf.Clamp(energy + restoreSpeed * Time.deltaTime, 0f, maxEnergy);
            //Debug.Log("~");
            freeSwap = true;
        }
        
    }
    void UpdateLostHPUI(float lossHP)
    {
        currDelayTimer = lossHPAnimDelay;
        startTimer = true;
    }
    void HandleLostHPUI()
    {

        if (currDelayTimer < 0f)
        {
            StartCoroutine(LossHPAnim());
            currDelayTimer = lossHPAnimDelay;
            startTimer = false;
        }
        if (startTimer)
            currDelayTimer -= Time.fixedDeltaTime;
    }
    IEnumerator LossHPAnim()
    {
        float timer = 0f;
        while (lossHPbar.fillAmount > (float)energy / maxEnergy)
        {
            timer += Time.fixedDeltaTime;
            lossHPbar.fillAmount = lossHPbar.fillAmount -
                                    lossHPAnimDuration;
            yield return new WaitForEndOfFrame();
        }
        lossHPbar.fillAmount = (float)energy / maxEnergy;
    }
    public bool Spend(float amount)
    {
        
        if (energy >= amount)
        {

            energy -= amount;
            UpdateLostHPUI(amount);
            touchFloorTimer = 0f;
            return true;
        }
        else
            return false;
    }

    void Start()
    {
        
    }
    float prevRealTime;
    void Update()
    {
        //Debug.Log(Time.timeScale);
        if (Time.timeScale < 0.8f)
        {
            bulletTimeTimer += Time.deltaTime;
            if (bulletTimeTimer > bulletTimeSpendDelay)
            {
                if (!Spend(bulletTimeCost))
                {
                    PlayerControl1.Instance.CancelAimBulletTime();
                }
            }
                
        }
        if (PlayerControl1.Instance.isTouchingGround && Time.timeScale > 0.8f && !Input.GetMouseButton(0))
        {
            bulletTimeTimer = 0f;
            Restore();
        }
        hpBar.fillAmount = (float)energy / maxEnergy;
        //Debug.Log(energy / maxEnergy);
        prevRealTime = Time.realtimeSinceStartup;
        HandleLostHPUI();
        if (energy < swapCost && !freeSwap)
        {
            lossHPbar.color = new Color(1f, 0.7f, 0.7f);
            fatigue.color = fatigueColor;
        }
        else
        {
            lossHPbar.color = Color.white;
            fatigue.color = new Color(1f,1f,1f,0f);
        }

        if (leftSwapNumText != null)
            HandleLeftSwapNumText();
    }

    void HandleLeftSwapNumText()
    { 
        if (swapCost == 0) return;
        leftSwapNumText.text = ((int)(energy / swapCost) + (freeSwap ? 1 : 0)).ToString() + " / " + ((int)(maxEnergy / swapCost) + 1).ToString();
    }
}
