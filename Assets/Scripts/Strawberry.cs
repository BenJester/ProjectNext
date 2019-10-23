using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strawberry : MonoBehaviour
{
    public bool active;
    public float animDuration;
    public Vector3 translation;
    public float rotationSpeed;

    SpriteRenderer sr;
    CheckPointTotalManager worldManager;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        worldManager = GameObject.FindGameObjectWithTag("WorldManager").GetComponent<CheckPointTotalManager>();
        worldManager.maxStrawberryCount += 1;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {

        if (col.CompareTag("player") && active)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(Anim(animDuration));
            worldManager.strawberryCount += 1;
            worldManager.SetStrawBerryText();
            active = false;
        }
    }

    IEnumerator Anim(float duration)
    {
        float curr = 0f;
        float originalAlpha = sr.color.a;
        while (curr <= 1f)
        {
            sr.color = new Color(1f, 1f, 1f, (1 - curr) * originalAlpha);
            transform.Translate(Time.deltaTime / duration * translation);
            transform.Rotate(0f, rotationSpeed, 0f);
            curr = Mathf.Clamp01(curr + Time.deltaTime / duration);
            yield return new WaitForEndOfFrame();
        }
    }
}
