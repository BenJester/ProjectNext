using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSprite : MonoBehaviour
{

    Transform player;
    SpriteRenderer thisSprite;
    SpriteRenderer playerSprite;

    Color color;

    [Header("时间控制")]
    public float activeTime;//
    public float activeStart;

    [Header("不透明度控制")]
    float alpha;
    public float alphaSet;
    public float alphaMultiplier;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();
        thisSprite = GetComponent<SpriteRenderer>();
        playerSprite = player.GetChild(0).GetComponent<SpriteRenderer>();

        alpha = alphaSet;
        thisSprite.sprite = playerSprite.sprite;
        transform.position = player.position;
        transform.localScale = player.localScale;
        transform.rotation = player.rotation;

        activeStart = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        alpha *= alphaMultiplier;
        color = new Color(.5f,.5f,1,alpha);
        thisSprite.color = color;
        if(Time.time >= activeStart + activeTime){
            //return to object pool
            ShadowPool.instance.ReturnPool(this.gameObject);
        }
    }

}
