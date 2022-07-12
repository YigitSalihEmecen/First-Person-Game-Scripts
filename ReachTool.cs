using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachTool : MonoBehaviour
{
    public float pivotRotation;
    public GameObject crosshair;
    private Vector3 crosshairScaleBig;
    private Vector3 crosshairScaleSmall;
    private bool upScale;
    private bool downScale;
    

    public enum ReachState
    {
        nothing,
        clothRack,
        broomPivot,
        button,
        clothHang,
        cabinClothesPivot,
    }

    public ReachState state;

    public Vector3 pivotPosition;
    void Start()
    {
        state = ReachState.nothing;
        pivotPosition = transform.position;
        crosshairScaleBig = new Vector3(3, 3, 3);
        crosshairScaleSmall = new Vector3(1, 1, 1);
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
        else if(other.CompareTag("cabinClothesPivot"))
        {
            state = ReachState.cabinClothesPivot;
            pivotPosition = other.transform.position;
            pivotRotation = other.transform.eulerAngles.z;
        }
        else if (other.CompareTag("button"))
        {
            state = ReachState.button;
        }
        else if (other.CompareTag("clothHang"))
        {
            state =ReachState.clothHang;
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
        else if (other.CompareTag("button"))
        {
            state = ReachState.nothing;
        }
        else if (other.CompareTag("clothHang"))
        {
            state = ReachState.nothing;
        }
        else if (other.CompareTag("cabinClothesPivot"))
        {
            state = ReachState.nothing;
        }
    }

    private void Update()
    {
        if (state != ReachState.nothing)
        {
            CrosshairUpScale();
        }
        else
        {
            CrosshairDownScale();
        }
    }

    void CrosshairUpScale()
    {
        crosshair.transform.localScale = Vector3.Lerp(crosshair.transform.localScale,crosshairScaleBig, 20 * Time.deltaTime);
    }
    void CrosshairDownScale()
    {
        crosshair.transform.localScale = Vector3.Lerp(crosshair.transform.localScale,crosshairScaleSmall, 20 * Time.deltaTime);
    }
}
