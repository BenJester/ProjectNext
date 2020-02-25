using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ti_GunType:MonoBehaviour
{
    // Start is called before the first frame update
    public Text ammoText;
    public int ammoMax;
    public int ammoNow;
    
    public bool canRotate = true;
    protected void Start()
    {
        ammoNow=ammoMax;
        SetAmmo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAmmo(){
        ammoText.text= ammoNow.ToString()+"/"+ammoMax.ToString();
        ammoNow= Mathf.Clamp(ammoNow,0,ammoMax);


        if(ammoNow==0) ammoText.color = Color.red;
        else ammoText.color = Color.green;

        
    }
}
