﻿using UnityEngine;
using System.Collections;
public class MultipleSpritesPlayer : MonoBehaviour
{

    [Header("将你想要播放的Gif放在Asset/Resources文件夹下")]
    public string gitFileName;
    private Coroutine AnimationPlay;//播放协程  
    private Sprite[] sprites;//被切割的序列图数组  
    private SpriteRenderer spriteRenderer;//精灵控制器  
    public float fps = 30;


    void Start()
    {
        sprites = Resources.LoadAll<Sprite>(gitFileName);//载入资源aa.png  
        spriteRenderer = GetComponent<SpriteRenderer>() as SpriteRenderer;//这东西在精灵身上自带的，直接拿就行  }

    }
    void Update()
    {
    }

    //按钮动作  
    public void PlayButtonOnClick()
    {
        AnimationPlay = StartCoroutine(AnimationPlayThread(fps));
    }
    public void StopButtonOnClick()
    { 
        StopCoroutine(AnimationPlay);
    }  
    //实质在控制什么时候在精灵上放那张被切割的小图而已  
    IEnumerator AnimationPlayThread(float fps)
    {
        int i = 0;  
        while (true)
        {
            if (i < sprites.Length)
            {
                spriteRenderer.sprite = sprites[i];
                i++;
            }
            else
            {
                i = 0;
            }  
            yield return new WaitForSeconds(1 / fps);
        }  
    }  
}