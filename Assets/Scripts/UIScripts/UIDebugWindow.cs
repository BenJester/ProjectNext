using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDebugWindow : MonoBehaviour
{
    private Text m_txt;
    // Start is called before the first frame update
    void Start()
    {
        m_txt = GetComponent<Text>();
        GlobalVariable.StaticUIDebugWindow = this;
        SetText(GMText.GetInstance().GetCurrentText());
    }

    public void SetText(string str)
    {
        m_txt.text = str;
    }
}
