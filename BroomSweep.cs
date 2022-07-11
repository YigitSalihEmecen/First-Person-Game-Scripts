using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BroomSweep : MonoBehaviour
{
    [Header("Things to Drag & Drop")]
    public PlayerController playerController;
    public ButtonPress buttonPress;
    public ReachTool ReachTool;
    public GameObject pickUpText;
    public GameObject taskSprite;
    public AudioSource pickUpSound;
    public AudioSource putDownSound;
    public AudioSource taskCompleteSound;
    public Transform target;
    public BoxCollider boxCollider;
    public Slider slider;
    
    [Header("Variables")]
    public float pickUpSpeed = 1;
    private float sweepTimer = 0;
    public float sweepTaskLenght;
    public bool inHands;
    public bool inReach;
    public bool puttingItBack;
    public bool isSweeping;
    public bool sweepDone;
    public bool questStarted;
    
    private Vector3 pivotPosition;
    private float pivotRotation;
    
    void Start()
    {
        pickUpText.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Reach") && inHands == false)
        {
            inReach = true;
            if (questStarted == true) pickUpText.SetActive(true);
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
        if (buttonPress.pressed == true)
        {
            questStarted = true;
            taskSprite.SetActive(true);
        }
        
        if (inReach && Input.GetButtonDown("Interact") && inHands == false && questStarted == true)
        {
            pickUpSound.Play();
            pickUpText.SetActive(false);
            inHands = true;
            inReach = false;
            boxCollider.enabled = false;
            puttingItBack = false;
        }

        else if (Input.GetButtonDown("Interact") && ReachTool.state == ReachTool.ReachState.broomPivot && inHands == true)
        {
            putDownSound.Play();
            pivotPosition = ReachTool.pivotPosition;
            pivotRotation = ReachTool.pivotRotation;
            puttingItBack = true;
            inHands = false;
            boxCollider.enabled = true;
            questStarted = false;
        }
        
        if (playerController.onDirtyFloor == true && Input.GetMouseButton(0) && sweepDone == false)
        {
            isSweeping = true;
            slider.gameObject.SetActive(true);
            Sweeping();
        }
        
        else isSweeping = false;
        
        if (inHands == true)HoldItem();
       
        if (puttingItBack == true && transform.position != pivotPosition)PutItemBack();
        else puttingItBack = false;
      
    }
    void HoldItem()
    {
        //lerping item to hand position
        transform.position = Vector3.Lerp(transform.position, target.position, pickUpSpeed * Time.deltaTime);
        
        if (isSweeping == false)
        {
            Vector3 newRotation = new Vector3(-90, 0, Mathf.Lerp(transform.eulerAngles.y, target.eulerAngles.y, 20 * Time.deltaTime));
            transform.eulerAngles = newRotation;
            //if i don't check if isSweeping is true or not, this part will interrupt with sweep animation.
        }
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
        //setting a sin motion and a timer
        sweepTimer += Time.deltaTime;
        slider.value = sweepTimer / sweepTaskLenght;
        transform.eulerAngles = new Vector3(-90 + (transform.eulerAngles.y + Mathf.Sin(sweepTimer*4)) * 10, 0, 0);
        
        if (sweepTimer >= sweepTaskLenght)
        {
            sweepDone = true;
            taskCompleteSound.Play();
            slider.gameObject.SetActive(false);
            taskSprite.SetActive(false);
        }
    }
}