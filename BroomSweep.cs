using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BroomSweep : MonoBehaviour
{
    [Header("Things to Drag & Drop")]
    public PlayerController playerController;
    public TaskTracker taskTracker;
    public Animator animator;
    public GameObject pickUpText;
    public GameObject dirtyFloor;
    public AudioSource pickUpSound;
    public AudioSource putDownSound;
    public AudioSource taskCompleteSound;
    public Transform handPosition;
    public BoxCollider boxCollider;
    public Slider slider;
    
    [Header("Variables")]
    public float pickUpSpeed = 1;
    private float sweepTimer = 0;
    public float sweepTaskLenght;
    public bool inHands;
    public bool puttingItBack;
    public bool isSweeping;
    public bool onDirtyFloor;

    private Vector3 pivotPosition;
    private float pivotRotation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("dirtyFloor"))
        {
            onDirtyFloor = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("dirtyFloor"))
        {
            onDirtyFloor = false;
        }
    }

    void Update()
    {
        PickUpTextHandler();
        BroomInteractions();
        
        if (onDirtyFloor == true && Input.GetMouseButton(0) && taskTracker.sweepTaskDone == false)
        {
            isSweeping = true;
            animator.SetBool("isSweeping", true);
            slider.gameObject.SetActive(true);
            Sweeping();
        }

        else
        {
            animator.SetBool("isSweeping",false);
            isSweeping = false;
        }
        
        if (inHands == true) HoldItem();
       
        if (puttingItBack == true && transform.position != pivotPosition) PutItemBack();
        else puttingItBack = false;
    }
    void HoldItem()
    {
        //lerping item to hand position
        transform.position =
            new Vector3(Mathf.Lerp(transform.position.x, handPosition.position.x, pickUpSpeed * Time.deltaTime),
                transform.position.y,
                Mathf.Lerp(transform.position.z, handPosition.position.z, pickUpSpeed * Time.deltaTime));
        transform.eulerAngles =
            new Vector3(transform.eulerAngles.x, handPosition.eulerAngles.y, transform.eulerAngles.z);
    }
    void PutItemBack()
    {
        //lerping item to pivot position
        transform.position = Vector3.Lerp(transform.position, pivotPosition, 5 * Time.deltaTime);
        Vector3 newRotation = new Vector3(-90, 0 ,Mathf.Lerp(transform.eulerAngles.z, pivotRotation, 20 * Time.deltaTime));
        transform.eulerAngles = newRotation;
    }
    void Sweeping()
    {
        sweepTimer += Time.deltaTime;
        slider.value = sweepTimer / sweepTaskLenght;

        if (sweepTimer >= sweepTaskLenght)
        {
            taskTracker.sweepTaskDone = true;
            taskCompleteSound.Play();
            slider.gameObject.SetActive(false);
            dirtyFloor.SetActive(false);
        }
    }
    void PickUpTextHandler()
    {
        if (playerController.itemInRange.gameObject.name == "Broom" && taskTracker.sweepTaskActivated == true)
        {
            pickUpText.SetActive(true);
        }
        else
        {
            pickUpText.SetActive(false);
        }

    }
    void BroomInteractions()
    {
        if (playerController.interactingItem.gameObject.name == "Broom" && inHands == false && taskTracker.sweepTaskActivated == true)
        {
            pickUpSound.Play();
            inHands = true;
            boxCollider.enabled = false;
            puttingItBack = false;
        }

        else if (playerController.interactingItem.gameObject.name == "BroomPivot" && inHands == true)
        {
            putDownSound.Play();
            pivotPosition = playerController.interactingItem.transform.position;
            pivotRotation = playerController.interactingItem.transform.eulerAngles.y;
            puttingItBack = true;
            inHands = false;
            boxCollider.enabled = true;
            slider.gameObject.SetActive(false);
        }
    }
}