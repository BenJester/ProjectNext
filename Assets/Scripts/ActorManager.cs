using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ActorManager : MonoBehaviour
{

    private SpriteRenderer black;
    Text text;
    //private static ActorManager _instance;
    //public static ActorManager Instance { 
    //	get { 	
    //		return _instance; 
    //	}
    //}

    //public static List<GameObject> enemies;
    //public static List<GameObject> obj;
    //public static List<GameObject> bullets;

    void Awake()
    {
        black = GameObject.FindGameObjectWithTag("black").GetComponent<SpriteRenderer>();
        text = GameObject.FindWithTag("Respawn").GetComponent<Text>();
        //if (ActorManager._instance == null) {
        //	ActorManager._instance = this;
        //	_instance.Init ();
        //} else {
        //	if (ActorManager._instance != this) {
        //		throw new InvalidOperationException ("Cannot have two instances of a Singleton");
        //	}
        //}
    }

    private void Init()
    {
        //enemies = new List<GameObject> ();
        //obj = new List<GameObject> ();
        //bullets = new List<GameObject> ();
    }


    void Start()
    {
        black.color = Color.white;

        StartCoroutine(FadeOutBlack(1f));
        StartCoroutine(FadeInText(0.1f, 1f, 0.2f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene(24);
        }
    }


    IEnumerator FadeOutBlack(float duration)
    {
        while (black.color.a > 0f)
        {
            black.color = new Color(black.color.r, black.color.g, black.color.b, Mathf.Clamp01(black.color.a - Time.deltaTime / duration));
            yield return new WaitForSeconds(0.05f);
        }
    }
    IEnumerator FadeInText(float duration1, float duration2, float duration3)
    {
        while (text.color.a < 1f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Clamp01(text.color.a + Time.deltaTime / duration1));
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(duration2);

        while (text.color.a > 0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Clamp01(text.color.a - Time.deltaTime / duration3));
            yield return new WaitForSeconds(0.02f);
        }
    }
}
