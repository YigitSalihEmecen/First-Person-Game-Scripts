using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public Animator animator;
    public GameObject openText;

    public AudioSource doorSound;

    public bool inReach;
    
    void Start()
    {
        inReach = false;
        animator.SetBool("Open", false);
        animator.SetBool("Closed", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Reach"))
        {
            inReach = true;
            openText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Reach"))
        {
            inReach = false;
            openText.SetActive(false);
        }
    }

    void Update()
    {
        if (inReach && Input.GetButtonDown("Interact") && animator.GetBool("Closed"))
        {
            DoorOpens();
        }
        else if (inReach && Input.GetButtonDown("Interact") && animator.GetBool("Open"))
        {
            DoorCloses();
        }
    }

    void DoorOpens()
    {
        animator.SetBool("Open", true);
        animator.SetBool("Closed", false);
        doorSound.Play();
    }

    void DoorCloses()
    {
        animator.SetBool("Open", false);
        animator.SetBool("Closed", true);
        doorSound.Play();
    }
    
}
