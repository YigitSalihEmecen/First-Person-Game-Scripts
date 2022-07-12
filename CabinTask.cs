using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CabinTask : MonoBehaviour
{
    public ButtonPress buttonPress;
    public ReachTool reachTool;
    public CabinTaskTrack cabinTaskTrack;
    public Transform handPosition;
    public GameObject parent;
    public MeshRenderer[] meshRenderer;
    public AudioSource tidyClothesSpawnSound;
    public AudioSource placedOnTableSound;
    public bool inReach;
    public bool taskDone;
    public bool taskStarted;
    public bool isPuttingBack;
    public bool inHands;
    public bool tableCheck;
    public float taskLenght;
    public float timer;
    public static int clothesPlaced = 0;
    public GameObject taskSprite1;
    public GameObject taskSprite2;
    public GameObject tidyClothesPrefab;
    public Slider slider;

    private Vector3 pivotPosition;
    private float pivotRotation;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Reach"))
        {
            inReach = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Reach"))
        {
            inReach = false;
        }
    }

    void Update()
    {
        TideUp();
        TaskDone();
        TaskFinish();
        HoldTidyClothes();
        PutTidyClothesBack();
        if (buttonPress.pressed == true)
        {
            taskStarted = true;
            taskSprite1.SetActive(true);
        }
        
    }

    void TideUp()
    {
        if (Input.GetMouseButton(0) && inReach == true && taskDone == false && taskStarted == true)
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
        taskSprite1.SetActive(false);
        taskSprite2.SetActive(true);
    }

    void HoldTidyClothes()
    {
        if (inHands == true)
        {
            parent.transform.position = Vector3.Lerp(parent.transform.position, handPosition.position, 25 * Time.deltaTime);
            parent.transform.eulerAngles = new Vector3(parent.transform.eulerAngles.x, handPosition.eulerAngles.y+30,
                parent.transform.eulerAngles.z);
        }
    }

    void PutTidyClothesBack()
    {
        if (Input.GetButtonDown("Interact") && inHands == true && reachTool.state == ReachTool.ReachState.cabinClothesPivot)
        {
            isPuttingBack = true;
            tableCheck = false;
            inHands = false;
            pivotPosition = reachTool.pivotPosition;
            pivotRotation = reachTool.pivotRotation;
            placedOnTableSound.Play();
            taskSprite2.SetActive(false);
            taskSprite1.SetActive(true);
        }

        if (isPuttingBack == true)
        {
            parent.transform.position = Vector3.Lerp(parent.transform.position, pivotPosition, 25 * Time.deltaTime);
        }

        if (parent.transform.position == pivotPosition && tableCheck == false)
        {
            isPuttingBack = false;
            taskDone = false;
            timer = 0;
            slider.value = 0;
            cabinTaskTrack.clothesPlacedOnTable += 1;
            tableCheck = true;
        }
    }

    void TaskFinish()
    {
        if (cabinTaskTrack.taskDone == true)
        {
            taskDone = true;
            taskSprite2.SetActive(false);
            taskSprite1.SetActive(false);
        }
    }
}

