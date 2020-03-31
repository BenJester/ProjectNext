using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public AudioClip clip;
    AudioSource source;
    public bool multiTrigger;
    bool triggered;
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    public void TriggerDialogue(){

        DialogueManager.instance.StartDialogue(dialogue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player") && (!triggered || multiTrigger))
        {
            TriggerDialogue();
            if (clip != null)
            {
                source.PlayOneShot(clip);
                
            }
            triggered = true;
        }
    }
}
