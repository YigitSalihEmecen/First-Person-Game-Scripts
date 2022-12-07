using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerTask : MonoBehaviour
{
    public PlayerController playerController;
    public TaskTracker taskTracker;
    public DialogueTrigger dialogueTrigger;
    public DialogueManager dialogueManager;
    public bool sentenceTrigger;
    public bool isOrderRight;
    public bool orderTaken;
    
    public customerOrder order;
    public enum customerOrder
    {
        sShirt,
        mShirt,
        lShirt,
    }
    
    void Update()
    {
        DialogueHandler();
        OrderHandler();
    }

    public void DialogueHandler()
    {
        if (playerController.interactingItem.gameObject == gameObject && sentenceTrigger == false)
        {
            dialogueTrigger.TriggerDialog();
            sentenceTrigger = true;
            orderTaken = true;
        }
        else if (Input.GetButtonDown("Jump") && sentenceTrigger == true)
        {
            dialogueManager.DisplayNextSentence();
        }

        if (dialogueManager.dialogActive == false && playerController.itemInRange.gameObject != gameObject)
        {
            //sentenceTrigger = false;
        }
    }

    public void OrderHandler()
    {
        if (playerController.itemInHand)
        {
            if (playerController.itemInHand.gameObject.CompareTag("item") && orderTaken == true)
            {
                if ((int) playerController.itemInHand.gameObject.GetComponent<ItemPickUp>().typeOfItem == (int) order)
                {
                    isOrderRight = true;
                }
                else
                {
                    isOrderRight = false;
                }
            }
        }
        else
        {
            isOrderRight = false;
        }

    }
}
