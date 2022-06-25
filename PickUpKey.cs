using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpKey : MonoBehaviour
{
    public GameObject realObject;
    public GameObject inventoryObject;
    public GameObject pickUpText;
    public AudioSource pickUpSound;
    public Transform target;
    public float pickUpSpeed = 1;
    public bool isPicking;

    public bool inReach;
    
    void Start()
    {
        inReach = false;
        pickUpText.SetActive(false);
        inventoryObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Reach"))
        {
            inReach = true;
            pickUpText.SetActive(true);
            
        }

        if (other.CompareTag("Inventory"))
        {
            realObject.SetActive(false);
            inventoryObject.SetActive(true);
            isPicking = false;
            pickUpText.SetActive(false);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Reach"))
        {
            inReach = false;
            pickUpText.SetActive(false);
        }
    }

    void Update()
    {
        if (inReach && Input.GetButtonDown("Interact"))
        {
            pickUpSound.Play();
            isPicking = true;
            pickUpText.SetActive(false);
            inReach = false;
            
        }

        if (isPicking == true)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, pickUpSpeed *Time.deltaTime);
        }
        
  
    }
}
