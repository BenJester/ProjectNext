using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public Text dialogueText;
    public Image photoImage;
    public Animator animator;

    //FIFO 
    Queue<string> sentences;

    void Awake()
    {
        if (instance == null) instance = this;
    }
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {


        animator.SetBool("isOpen", true);
        Debug.Log("starting conversation with" + dialogue.name);

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        photoImage.sprite = dialogue.photo;
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        StartCoroutine(autoNext());
    }
    public float delay = 1.4f;
    IEnumerator autoNext()
    {
        yield return new WaitForSeconds(delay);
        DisplayNextSentence();
    }

    void EndDialogue()
    {
        animator.SetBool("isOpen", false);
        Debug.Log("End the Conversation");
    }


    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return null;
        }
    }


}
