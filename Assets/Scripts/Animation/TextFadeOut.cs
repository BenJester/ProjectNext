using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFadeOut : MonoBehaviour
{
    public float dur;
    public float delay;
    public float fadeInDur;
    public float fadeInDelay;
    float a;
    Text text;
    void Start()
    {
        text = GetComponent<Text>();
        a = text.color.a;
        StartCoroutine(DoFadeOut());
        StartCoroutine(DoFadeIn());
    }
    IEnumerator DoFadeOut()
    {
        
        yield return new WaitForSeconds(delay);
        float timer = 0f;
        
        while (timer < dur)
        {
            timer += Time.deltaTime;
            text.color = new Color(text.color.r, text.color.b, text.color.g, (1 - timer / dur) * a);
            yield return new WaitForEndOfFrame();
        }
        text.color = new Color(text.color.r, text.color.b, text.color.g, 0f);
    }
    IEnumerator DoFadeIn()
    {
        
        text.color = new Color(text.color.r, text.color.b, text.color.g, 0f);
        yield return new WaitForSeconds(fadeInDelay);
        float timer = 0f;
        
        while (timer < fadeInDur)
        {
            timer += Time.deltaTime;
            text.color = new Color(text.color.r, text.color.b, text.color.g, (timer / fadeInDur) * a);
            yield return new WaitForEndOfFrame();
        }
        text.color = new Color(text.color.r, text.color.b, text.color.g, a);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
