using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPool : MonoBehaviour
{
    public static ShadowPool instance;
    public GameObject shadowPrefab;
    public int shadowCount;

    Queue<GameObject> availableObject = new Queue<GameObject>();

    void Awake()
    {
        if(instance==null) instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void FillPool(){

        for (int i = 0;i < shadowCount; i++) {
            var newShadow = Instantiate(shadowPrefab);
            newShadow.transform.SetParent(transform);

            //取消启用，返回对象池
            ReturnPool(newShadow);
        }
    }

    public void ReturnPool(GameObject go){
        go.SetActive(false);
        availableObject.Enqueue(go);
    }

    public GameObject GetFromPool(){
        if(availableObject.Count == 0){
            FillPool();
        }
        var outShadow = availableObject.Dequeue();
        outShadow.SetActive(true);
        return outShadow;

    }
}
