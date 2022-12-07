using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjects : Interactable
{
    public bool objectOnFocus;
    public bool interacctedWithObject;
    public override void OnFocus()
    {
        objectOnFocus = true;
    }

    public override void OnInteract()
    {
        interacctedWithObject = true;
    }

    public override void OnLoseFocus()
    {
        objectOnFocus = false;
    }
}
