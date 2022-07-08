using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachTool : MonoBehaviour
{
    public bool canReach;
    public float pivotRotation;
    public GameObject pivot;

    public Vector3 pivotPosition;
    void Start()
    {
        canReach = false;
        pivotPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("pivot"))
        {
            canReach = true;
            pivotPosition = other.transform.position;
            pivotRotation = other.transform.eulerAngles.y;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("pivot"))
        {
            canReach = false;
        }
    }
}
