using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject pickUpText;
    public Transform temporaryPivot;
    public AudioSource pickUpSound;
    public AudioSource putDownSound;
    public Transform handPosition;
    public BoxCollider boxCollider;
    public float pickUpSpeed = 1;
    public bool inHands;
    public bool puttingItBack;
    
    public itemType typeOfItem;
    public enum itemType
    {
        sShirt,
        mShirt,
        lShirt,
    }
    
    void Update()
    {
        if (playerController.interactingItem.gameObject == gameObject && inHands == false && playerController.handIsFull == false)
        {
            pickUpSound.Play();
            pickUpText.SetActive(false);
            playerController.itemInHand = gameObject;
            inHands = true;
            playerController.handIsFull = true;
            boxCollider.enabled = false;
            puttingItBack = false;
        }

        else if (playerController.interactingItem.gameObject.CompareTag("pivot") && inHands == true)
        {
            putDownSound.Play();
            playerController.itemInHand = null;
            temporaryPivot = playerController.interactingItem.transform;
            puttingItBack = true;
            inHands = false;
            playerController.handIsFull = false;
            boxCollider.enabled = true;
        } 
        
        else if (playerController.interactingItem.gameObject.CompareTag("customer") && inHands == true &&
                 playerController.itemInRange.gameObject.GetComponent<CustomerTask>().isOrderRight == true)
        {
            putDownSound.Play();
            playerController.itemInHand = null;
            temporaryPivot = playerController.itemInRange.transform.GetChild(0).transform;
            puttingItBack = true;
            inHands = false;
            playerController.handIsFull = false;
            boxCollider.enabled = true;
        }

        if (inHands == true)
        {
            HoldItem();
        }
        
        if (puttingItBack == true)
        {
            PutItemBack();
        }
    }
    void HoldItem()
    {
        //lerping item to hand position
        transform.position = Vector3.Lerp(transform.position, handPosition.position, pickUpSpeed * Time.deltaTime);
        Vector3 newRotation = new Vector3(0, Mathf.LerpAngle(transform.eulerAngles.y, handPosition.eulerAngles.y +90, 20 * Time.deltaTime), 0);
        transform.eulerAngles = newRotation;
    }
    void PutItemBack()
    {
        //lerping item to pivot position
        transform.position = Vector3.Lerp(transform.position, temporaryPivot.position, 30 * Time.deltaTime);
        Vector3 newRotation = new Vector3(0, Mathf.Lerp(transform.eulerAngles.y, temporaryPivot.eulerAngles.y, 20 * Time.deltaTime), 0);
        transform.eulerAngles = newRotation;
        if (transform.position == temporaryPivot.position)
        {
            puttingItBack = false;
        }
    }

    void GiveItemToCustomer()
    {
        
    }
}