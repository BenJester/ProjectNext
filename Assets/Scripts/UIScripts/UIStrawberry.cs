using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStrawberry : MonoBehaviour
{
    public Text TxtStrawberry;
    // Start is called before the first frame update
    void Start()
    {
        int a = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        CheckPointTotalManager.instance.UnregisteStrawberryUI(UpdateStrawberryText);
    }
    private void Awake()
    {
        if(CheckPointTotalManager.instance != null)
        {
            RegisteLate();
        }
    }
    public void RegisteLate()
    {
        CheckPointTotalManager.instance.RegisteStrawberryUI(UpdateStrawberryText);
        CheckPointTotalManager.instance.SetStrawBerryText();
    }
    public void UpdateStrawberryText(int nCurCnt, int nMaxCnt)
    {
        if(TxtStrawberry != null)
        {
            TxtStrawberry.text = string.Format("{0}/{1}", nCurCnt, nMaxCnt);
        }
    }
}
