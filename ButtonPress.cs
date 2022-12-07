using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public Animator animator;
    public PlayerController playerController;
    public AudioSource pressSound;
    public GameObject pressText;
    public bool pressed;
    
    void Update()
    {
        if (playerController.itemInRange.gameObject.CompareTag("button"))
        {
            pressText.SetActive(true);
        }
        else
        {
            pressText.SetActive(false);
        }
        
        if (playerController.interactingItem.gameObject.GetInstanceID() == gameObject.GetInstanceID() && true && Input.GetButtonDown("Interact"))
        {
            animator.SetBool("Pressed", true);
            pressSound.Play();
            pressed = true;
        }

        else if (Input.GetButtonUp("Interact"))
        {
            animator.SetBool("Pressed", false);
            pressed = false;
        }
    }
}
