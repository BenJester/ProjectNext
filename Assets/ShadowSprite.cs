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
        player = PlayerControl1.Instance.GetComponent<Transform>();
        thisSprite = GetComponent<SpriteRenderer>();
        playerSprite = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;
        thisSprite.sprite = playerSprite.sprite;
        transform.position = player.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
