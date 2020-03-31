using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public float dur;
    public float delay;
    SpriteRenderer sr;
    float a;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = Color.white;
        a = sr.color.a;
        StartCoroutine(DoFadeOut());
    }

    IEnumerator DoFadeOut()
    {
        yield return new WaitForSeconds(delay);
        float timer = 0f;
        while (timer < dur)
        {
            timer += Time.deltaTime;
            sr.color = new Color(sr.color.r, sr.color.b, sr.color.g, (1 - timer / dur) * a);
            yield return new WaitForEndOfFrame();
        }
        sr.color = new Color(sr.color.r, sr.color.b, sr.color.g, 0f);
    }
    void Update()
    {
        
    }
}
