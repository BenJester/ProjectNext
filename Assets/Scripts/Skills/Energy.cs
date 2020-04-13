using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Energy : Skill
{
    public float maxEnergy;
    public float energy;
    public float restoreDelay;
    public float bulletTimeSpendDelay;
    public float bulletTimeTimer;
    public float restoreSpeed;
    public float swapCost;
    public float bulletTimeCost;
    public Image hpBar;

    public Image lossHPbar;

    public float lossHPAnimDelay;
    public float lossHPAnimDuration;
    float currDelayTimer;
    bool startTimer;
    
    bool init;
    public static Energy Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }
    public void Restore()
    {
        energy = Mathf.Clamp(energy + restoreSpeed * Time.deltaTime, 0f, maxEnergy);
        //Debug.Log("~");
        
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
            return true;
        }
        else
            return false;
    }

    void Start()
    {
        
    }

    void Update()
    {
        Debug.Log(Time.timeScale);
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
        
        HandleLostHPUI();
    }
}
