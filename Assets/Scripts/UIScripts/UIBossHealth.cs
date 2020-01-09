using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBossHealth : MonoBehaviour
{
    public Color CrHealth;
    public Color CrWeak;

    public Image ImgWeak;
    public Image ImgHealth;
    // Start is called before the first frame update
    void Start()
    {
        OpenBossHealthBar(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealth(float fRate)
    {
        ImgHealth.fillAmount = fRate;
    }
    public void OpenBossHealthBar(bool bOpen)
    {
        ImgHealth.gameObject.SetActive(bOpen);
        ImgWeak.gameObject.SetActive(bOpen);
    }
}
