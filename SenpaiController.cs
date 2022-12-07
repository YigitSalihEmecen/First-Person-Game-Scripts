using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenpaiController : MonoBehaviour
{
    public PlayerController playerController;
    public DialogueManager dialogueManager;
    public DialogueTrigger dialogueTrigger;
    public bool sentenceTrigger;
    
    public enum senpaiState
    {
        busy,
        dialog1,
        dialog2,
        dialog3,
    }

    public senpaiState state;

    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        DialogHandler();
    }

    private void DialogHandler()
    {
        if (playerController.interactingItem.gameObject == gameObject && sentenceTrigger == false)
        {
            dialogueTrigger.TriggerDialog();
            sentenceTrigger = true;
        }
        else if (Input.GetButtonDown("Jump") && sentenceTrigger == true)
        {
            dialogueManager.DisplayNextSentence();
        }

        if (dialogueManager.dialogActive == false && playerController.interactingItem.gameObject != gameObject)
        {
            //sentenceTrigger = false;
        }
    }
    
}
