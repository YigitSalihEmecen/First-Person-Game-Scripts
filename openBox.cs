using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openBox : MonoBehaviour
{
    public Animator animator;
    public GameObject keyOBNeeded;
    public GameObject openText;
    public GameObject keyMissingText;
    public AudioSource openSound;
    public AudioSource errorSound;
    
    public bool inReach;
    public bool isOpen;
    void Start()
    {
        inReach = false;
        openText.SetActive(false);
        keyMissingText.SetActive(false);
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
            keyMissingText.SetActive(false);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (keyOBNeeded.activeInHierarchy == true && inReach && Input.GetButtonDown("Interact"))
        {
            keyOBNeeded.SetActive(false);
            openSound.Play();
            animator.SetBool("open", true);
            openText.SetActive(false);
            keyMissingText.SetActive(false);
            isOpen = true;
        }
        else if (keyOBNeeded.activeInHierarchy == false && inReach && Input.GetButtonDown("Interact"))
        {
            openText.SetActive(false);
            keyMissingText.SetActive(true);
            errorSound.Play();
            
        }
    }
}
