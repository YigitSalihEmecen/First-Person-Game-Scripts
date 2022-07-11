using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ReachTool ReachTool;
    public HangClothesTask hangClothesTask;
    public GameObject realObject;
    public GameObject pickUpText;
    public AudioSource pickUpSound;
    public AudioSource putDownSound;
    public Transform target;
    public BoxCollider boxCollider;
    public float pickUpSpeed = 1;
    public bool inHands;
    public bool inReach;
    public bool puttingItBack;
    
    private Vector3 pivotPosition;
    private float pivotRotation;

    
    void Start()
    {
        inReach = false;
        inHands = false;
        pickUpText.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Reach") && inHands == false)
        {
            inReach = true;
            pickUpText.SetActive(true);
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
        if (inReach && Input.GetButtonDown("Interact") && inHands == false)
        {
            pickUpSound.Play();
            pickUpText.SetActive(false);
            inHands = true;
            inReach = false;
            boxCollider.enabled = false;
            puttingItBack = false;
        }

        else if (Input.GetButtonDown("Interact") && ReachTool.state == ReachTool.ReachState.clothRack && inHands == true)
        {
            putDownSound.Play();
            pivotPosition = ReachTool.pivotPosition;
            pivotRotation = ReachTool.pivotRotation;
            puttingItBack = true;
            inHands = false;
            hangClothesTask.questProgression += 1;
            //boxCollider.enabled = true;
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
        transform.position = Vector3.Lerp(transform.position, target.position, pickUpSpeed * Time.deltaTime);
        Vector3 newRotation = new Vector3(0, Mathf.Lerp(transform.eulerAngles.y, target.eulerAngles.y, 20 * Time.deltaTime), 0);
        transform.eulerAngles = newRotation;
    }
    void PutItemBack()
    {
        //lerping item to pivot position
        transform.position = Vector3.Lerp(transform.position, pivotPosition, 5 * Time.deltaTime);
        Vector3 newRotation = new Vector3(0, Mathf.Lerp(transform.eulerAngles.y, pivotRotation, 20 * Time.deltaTime), 0);
        transform.eulerAngles = newRotation;
    }
}