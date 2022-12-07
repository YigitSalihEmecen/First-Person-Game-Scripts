using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTracker : MonoBehaviour
{
    [Header("General")] 
    public AudioSource taskDoneSound;
    public bool inHands;
    
    [Header("Cabin Task")]
    public bool cabinTaskActivated;
    public bool cabinTaskDone;
    private bool hasPlayedCabinTaskDoneSound;
    public int clothesPlacedOnTable;
    
    [Header("Floor Sweep Task")]
    public bool sweepTaskActivated;
    public bool sweepTaskDone;
    private bool hasPlayedSweepTaskDoneSound;

    [Header("Placing Clothes Task")]
    public bool placingTaskActivated;
    public bool placingDone;
    private bool hasPlayedPlacingDoneSound;
    public int clothesPlaced;
    
    [Header("Customer Task")]
    public bool customerTaskActivated;
    public GameObject clothesInHand;

    void Update()
    {
        CabinTaskHandler();
        PlacingClothesTaskHandler();
        CustomerTaskHandler();
        SweepTaskHandler();
    }

    void CabinTaskHandler()
    {
        if (clothesPlacedOnTable == 3)
        {
            cabinTaskDone = true;
            if (hasPlayedCabinTaskDoneSound == false)
            {
                taskDoneSound.Play();
                hasPlayedCabinTaskDoneSound = true;
            }
        }
    }

    void PlacingClothesTaskHandler()
    {
        if (clothesPlaced == 4)
        {
            placingDone = true;
            if (hasPlayedPlacingDoneSound == false)
            {
                taskDoneSound.Play();
                hasPlayedPlacingDoneSound = true;
            }
        }

    }

    void CustomerTaskHandler()
    {

    }

    void SweepTaskHandler()
    {
        //bruh
    }
}
