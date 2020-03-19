using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxChoicer : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("一个选择不同奖励的选择器，直接在子物件放置Box_Pickup就可以了")]
    
    public bool isTrigger = false;
    private int boxCount;
    private GameObject[] boxs;
    void Start()
    {
        boxCount=transform.childCount;
        boxs= new GameObject[boxCount];
        for (int i = 0; i < boxCount; i++)
        {
            boxs[i]= transform.GetChild(i).gameObject;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTrigger)
        {
            foreach (GameObject box in boxs)
            {
                if (box == null)
                {
                    isTrigger = true;
                    Triggered();
                }
            }
        }

    }

    void Triggered(){
        foreach (var item in boxs)
        {
            if(item!=null){
                Destroy(item);
            }
        }
    }
}
