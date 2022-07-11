using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachTool : MonoBehaviour
{
    public float pivotRotation;

    public enum ReachState
    {
        nothing,
        clothRack,
        broomPivot,
    }

    public ReachState state;

    public Vector3 pivotPosition;
    void Start()
    {
        state = ReachState.nothing;
        pivotPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("pivot"))
        {
            state = ReachState.clothRack;
            pivotPosition = other.transform.position;
            pivotRotation = other.transform.eulerAngles.y;
        }
        else if (other.CompareTag("broomPivot"))
        {
            state = ReachState.broomPivot;
            pivotPosition = other.transform.position;
            pivotRotation = other.transform.eulerAngles.z;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("pivot"))
        {
            state = ReachState.nothing;
        }
        else if (other.CompareTag("broomPivot"))
        {
            state = ReachState.nothing;
        }
    }
}
