using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariable 
{
    public static UIPlayerController s_UIPlayerCtrl;

    public static UIPlayerController GetUIPlayerCtrl()
    {
        if( s_UIPlayerCtrl == null )
        {
            s_UIPlayerCtrl = GameObject.FindObjectOfType<UIPlayerController>();
        }
        return s_UIPlayerCtrl;
    }
}
