using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBallon : MonoBehaviour
{

    public SpriteRenderer spr1;
    public SpriteRenderer spr2;

    public GameObject particle;
    public GameObject nextObj;
    AudioSource audioSource;
    bool veloTemp;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "ballon"){
            spr1.color = Color.yellow;
            spr2.color = Color.yellow;
            veloTemp = other.GetComponent<Rigidbody2D>().velocity.normalized.x >= 0;
        }
    }

  
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "ballon" && other.GetComponent<Rigidbody2D>().velocity.normalized.x >= 0 ==veloTemp){
            spr1.color = Color.green;
            spr2.color = Color.green;
            audioSource.Play();
            GameObject part = Instantiate(particle,transform.position,Quaternion.identity);
            other.GetComponent<Rigidbody2D>().velocity *= 1.5f;
            Destroy(part,.8f);
            if(nextObj!=null) nextObj.SetActive(true);
        }
    }
}
