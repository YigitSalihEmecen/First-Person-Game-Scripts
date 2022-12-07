using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CabinTask : MonoBehaviour
{
    [Header("Scripts")]
    public TaskTracker taskTracker;
    public PlayerController playerController;
    
    [Header("Drag & drop")] 
    public Transform handPosition;
    public Slider slider;
    public MeshRenderer[] meshRenderer;

    [Header("Spawn")] 
    public GameObject parent;
    public GameObject tidyClothesPrefab;
    
    [Header("Audio")] 
    public AudioSource tidyClothesSpawnSound;
    public AudioSource placedOnTableSound;
    
    [Header("Variables")] 
    public float taskLenght;
    public float timer;
    public bool taskDone;
    public bool isPuttingBack;
    public bool inHands;
    public bool tableCheck;
    
  

    private Vector3 pivotPosition;
    private float pivotRotation;
    
    void Update()
    {
        if(playerController.itemInRange.gameObject.GetInstanceID() == gameObject.GetInstanceID() )
        TideUp();
        TaskDone();
        HoldTidyClothes();
        PutTidyClothesBack();
    }

    void TideUp()
    {
        if (Input.GetMouseButton(0) && playerController.itemInRange.gameObject.CompareTag("scatteredClothes") && taskDone == false && taskTracker.cabinTaskActivated == true)
        {
            timer += Time.deltaTime;
            slider.gameObject.SetActive(true);
            slider.value = timer * 0.33f;
            
            if (timer >= taskLenght)
            {
                taskDone = true;
                SpawnTidyClothes();
            }
        }
        else
        {
            timer = 0;
            slider.gameObject.SetActive(false);
        }
    }

    void TaskDone()
    {
        if (taskDone == true)
        {
            for(int i = 0; i < meshRenderer.Length; i++)
            {
                meshRenderer[i].enabled = false;
            }
        }
    }

    void SpawnTidyClothes()
    {
        GameObject tidyClothes = Instantiate(tidyClothesPrefab, parent.transform);
        parent = tidyClothes;
        inHands = true;
        tidyClothesSpawnSound.Play();
    }

    void HoldTidyClothes()
    {
        if (inHands == true)
        {
            parent.transform.position = Vector3.Lerp(parent.transform.position, handPosition.position, 20 * Time.deltaTime);
            parent.transform.eulerAngles = new Vector3(parent.transform.eulerAngles.x, handPosition.eulerAngles.y+30,
                parent.transform.eulerAngles.z);
        }
    }

    void PutTidyClothesBack()
    {
        if (playerController.interactingItem.gameObject.CompareTag("cabinClothesPivot") && inHands == true)
        {
            isPuttingBack = true;
            tableCheck = false;
            inHands = false;
            pivotPosition = playerController.interactingItem.gameObject.transform.position;
            pivotRotation = playerController.interactingItem.gameObject.transform.eulerAngles.y;
            placedOnTableSound.Play();
        }

        if (isPuttingBack == true)
        {
            parent.transform.position = Vector3.Lerp(parent.transform.position, pivotPosition, 30 * Time.deltaTime);
            parent.transform.eulerAngles = new Vector3(parent.transform.eulerAngles.x,
                Mathf.Lerp(parent.transform.eulerAngles.y, pivotRotation, 20 * Time.deltaTime),
                parent.transform.eulerAngles.z);
        }

        if (parent.transform.position == pivotPosition && tableCheck == false)
        {
            isPuttingBack = false;
            taskDone = false;
            taskTracker.clothesPlacedOnTable += 1;
            tableCheck = true;
        }
    }
}

