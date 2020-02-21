using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPText : MonoBehaviour
{
    public Enemy enemy;
    public Text hpText;
    public Image hpBar;

    public Image lossHPbar;

    public float lossHPAnimDelay;
    public float lossHPAnimDuration;
    float currDelayTimer;
    bool startTimer;
    bool init;
    private void Start()
    {
        
        
    }
    private void LateInit()
    {
        enemy.OnLoseHP += UpdateLostHPUI;
        init = true;
    }
    void UpdateLostHPUI(int lossHP)
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
        while (lossHPbar.fillAmount > (float)enemy.health / enemy.maxHealth)
        {
            timer += Time.fixedDeltaTime;
            lossHPbar.fillAmount = lossHPbar.fillAmount -
                                    lossHPAnimDuration;
            yield return new WaitForEndOfFrame();
        }
        lossHPbar.fillAmount = (float)enemy.health / enemy.maxHealth;
    }

    private void Update()
    {
        if (!init) LateInit();
        hpText.text = enemy.health + " / " + enemy.maxHealth;
        hpBar.fillAmount =  (float)enemy.health / enemy.maxHealth;

        HandleLostHPUI();
    }
}
