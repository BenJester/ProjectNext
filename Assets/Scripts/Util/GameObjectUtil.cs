using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectUtil 
{
    public static void SafeSetActive(bool bActive,GameObject obj)
    {
        if(bActive == true)
        {
            if(obj.activeSelf == false)
            {
                obj.SetActive(true);
            }
        }
        else
        {
            if (obj.activeSelf == true)
            {
                obj.SetActive(false);
            }
        }
    }
}
