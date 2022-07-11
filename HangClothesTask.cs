using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HangClothesTask : MonoBehaviour
{
    public ButtonPress buttonPress;
    public GameObject questSprite;
    public AudioSource questCompleteSound;
    public Text taskText;
    public bool questStart = false;
    public int questProgression = 0;
    
    void Update()
    {
        QuestStartCheck();

        if (questStart == true)
        {
            questSprite.SetActive(true);
            Quest();
        }
    }

    void QuestStartCheck()
    {
        if (buttonPress.pressed == true)
        {
            questStart = true;
        }
    }
    
    void Quest()
    {
        taskText.text = String.Format("Place wood plates into the cabinet {0}/4", questProgression);
        
        if (questProgression == 4)
        {
            questSprite.SetActive(false);
            questCompleteSound.Play();
            questStart = false;
        }
    }
}
