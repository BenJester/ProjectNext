using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimHelper : MonoBehaviour
{
    public static AnimHelper Instance { get; private set; }

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        //thing = actor.GetComponent<Thing>();
    }

    public void Scale(GameObject obj, float startScale, float endScale, float duration)
    {
        Instance.StartCoroutine(DoScale(obj, startScale, endScale, duration));
    }

    IEnumerator DoScale(GameObject obj, float startScale, float endScale, float duration)
    {
        Vector3 originalScale = obj.transform.localScale;
        Vector3 s = obj.transform.localScale;
        obj.transform.localScale = new Vector3(startScale * s.x, startScale * s.y, originalScale.z);
        
        float curr = 0f;
        while (curr < duration)
        {
            s = obj.transform.localScale;
            float step = (endScale - startScale) * Time.deltaTime / duration;
            obj.transform.localScale = new Vector3(s.x + (step * originalScale.x), s.y + (step * originalScale.y), originalScale.z);
            curr += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        obj.transform.localScale = originalScale * endScale;
    }
}
