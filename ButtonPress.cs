using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public Animator animator;
    public AudioSource pressSound;
    public GameObject pressText;
    public bool pressed;
    public bool inReach;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Reach"))
        {
            inReach = true;
            pressText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Reach"))
        {
            inReach = false;
            pressText.SetActive(false);
        }
    }

    void Update()
    {
        if (inReach == true && Input.GetButtonDown("Interact"))
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
