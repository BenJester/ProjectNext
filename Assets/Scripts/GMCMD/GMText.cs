using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMText 
{
    private string m_strCurrent;
    public static GMText s_gmText = null;
    public static GMText GetInstance()
    {
        if(s_gmText == null)
        {
            s_gmText = new GMText();
        }
        return s_gmText;
    }
    public void AddText(string str)
    {
        m_strCurrent = m_strCurrent + "\n" + str;
        if(GlobalVariable.StaticUIDebugWindow != null)
        {
            GlobalVariable.StaticUIDebugWindow.SetText(m_strCurrent);
        }
    }
    public string GetCurrentText()
    {
        return m_strCurrent;
    }
}
