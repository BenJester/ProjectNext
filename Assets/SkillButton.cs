using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkillButton : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isPress = false;
    public Sprite pressedImage;
    public float pressTime = 0.1f;

    public GameObject particle;
    public GameObject onParticle;

    public bool hasNext;
    public GameObject nextButton;
    public UnityEvent nextAction;
    public AudioSource audio;
    
    float temp = 0;
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isPress) {
            if (collision.tag == "player")
            {
                temp += Time.deltaTime;
                if (temp >= pressTime)
                {
                    isPress = true;

                    if (hasNext && nextButton!=null) {
                        nextButton.SetActive(true);
                    }
                    nextAction.Invoke();

                    GetComponent<SpriteRenderer>().sprite = pressedImage;
                    GameObject part = Instantiate(particle, transform.position, Quaternion.identity);
                    audio.Play();
                    Destroy(part, 1f);

                    
                }
            }
        }
        
    }

    private void OnEnable()
    {
        GameObject part = Instantiate(onParticle, transform.position, Quaternion.identity);
        Destroy(part, 1f);
    }
}
