using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinTaskTrack : MonoBehaviour
{
    public int clothesPlacedOnTable;
    public bool taskDone;
    public AudioSource taskDoneSound;
    private bool hasPlayedDoneSound;
    void Update()
    {
        if (clothesPlacedOnTable == 3 && hasPlayedDoneSound == false)
        {
            taskDone = true;
            taskDoneSound.Play();
            hasPlayedDoneSound = true;
        }
    }
}
