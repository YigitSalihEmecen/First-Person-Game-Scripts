using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;

    public PlayerController playerController;
    public Text nameText;
    public Text dialogueText;
    public GameObject dialogueSprite;
    public bool dialogActive;
    void Start()
    {
        sentences = new Queue<string>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //playerController.gameObject.GetComponent<PlayerController>().enabled = false;
        dialogActive = true;
        dialogueSprite.gameObject.SetActive(true);
         
        nameText.text = dialogue.name;
        
        sentences.Clear();
        
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

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
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.03f);
        }
        
    }

    public void EndDialogue()
    {
        playerController.gameObject.GetComponent<PlayerController>().enabled = true;
        dialogActive = false;
        dialogueSprite.gameObject.SetActive(false);
    }
}
