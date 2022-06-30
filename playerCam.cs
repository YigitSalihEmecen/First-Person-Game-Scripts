using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCam : MonoBehaviour
{
    PlayerController playerController;
    public GameObject player;
    
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    [Header("Field of view")] 
    [SerializeField] Camera cameraF;

    [Range(0f, 50f)][SerializeField] private float FOVIncrease;
    [Range(0f, 10f)][SerializeField] private float acceleration;
    [Range(0f, 10f)][SerializeField] private float deceleration;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerController = player.GetComponent<PlayerController>();
    }

    
    private void Update()
    {
        //get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        
        dynamicFov();

    }

    void dynamicFov()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetButton("Vertical") && playerController.crouching == false)
        {
            cameraF.fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, 60 + FOVIncrease, acceleration * Time.deltaTime);
        }
        else
        {
            cameraF.fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, 60, deceleration * Time.deltaTime);
        }
    }
}
