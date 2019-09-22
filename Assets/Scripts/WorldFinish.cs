using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WorldFinish : MonoBehaviour
{
    public Text text;
    public CheckPointTotalManager worldManager;

    public void OnTriggerEnter2D(Collider2D col)
    {

        if (col.CompareTag("player"))
        {
            text.text = "Congratulations! \nStrawberries: " + worldManager.strawberryCount.ToString()
                      + "/" + worldManager.maxStrawberryCount.ToString() + "\nMade by yzt and yjc\n09.22.19\nbest of yjc: 127s\nbest of yzt: 200s";
        }
    }
}
