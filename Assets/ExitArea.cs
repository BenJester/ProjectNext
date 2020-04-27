using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitArea : MonoBehaviour
{

    public Hostage hostage;
    public bool isTrigger = false;
    public GameObject saveParticle;
    public GameObject saveImage;
    public SpriteRenderer image;
    public Door_Lion door;
    public AudioSource audioPlayer;
    

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTrigger && collision.gameObject == hostage.gameObject) {
            isTrigger = true;
            SaveHostage();

        }
    }

    void SaveHostage() {
        GetComponent<AudioSource>().Play();
        audioPlayer.Play();
        hostage.gameObject.SetActive(false);
        GameObject ins = Instantiate(saveParticle, transform.position, Quaternion.identity);
        Destroy(ins, 1f);
        saveImage.SetActive(true);
        image.color = Color.white;

        if (door != null) door.Open();

    }
}
