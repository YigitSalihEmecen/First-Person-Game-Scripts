using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementAdvanced : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slopeSpeed;
    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;
    public LayerMask whatIsCeiling;
    public bool ceilingCheck;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    
    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Headbob")] 
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.5f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 1f;
    [SerializeField] private float crouchBobSpeed = 10f;
    [SerializeField] private float crouchBobAmount = 0.5f;
    [SerializeField] private GameObject playerCam;
    

    private float defaultYPos;
    private float timer;

    
    
    [Header("Footstep Sounds")] 
    [SerializeField] private AudioSource woodClips;
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private float volumeChangeMultiplier = 0.5f;
    [SerializeField] private float pitchChangeMultiplier = 0.5f;
    
    public Transform orientation;
    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    public float sinFunction;
    public int sinState;
    public bool hasPlayedWalkingSound;

    public MovementState state;
    public enum MovementState
    {
        standing,
        walking,
        sprinting,
        crouching,
        crouchWalking,
        air,
        
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        startYScale = transform.localScale.y;
        woodClips = GetComponent<AudioSource>();
        defaultYPos = playerCam.transform.localPosition.y;
        
      

    }
    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        MyInput();
        CrouchCheck();
        SpeedControl();
        StateHandler();
        DragControl();
        HandleHeadBobAndFootstepSounds();
        
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void CrouchCheck()
    {
        //start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey) && ceilingCheck == false)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }
    private void MyInput()
    {
        // when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();
            
            //jump delay
            Invoke(nameof(ResetJump), jumpCooldown);
        }

    }
    private void StateHandler()
    {
        // Mode - Crouching
        if (Input.GetKey(crouchKey) && rb.velocity.magnitude < 1)
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        // Mode - Crouch Walking
        else if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouchWalking;
            moveSpeed = crouchSpeed;
        } 
        // Mode - Sprinting
        else if(grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        // Mode - Standing
        else if (grounded && rb.velocity.magnitude < 1)
        {
            state = MovementState.standing;
            moveSpeed = walkSpeed;
        }
        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        } 
        // Mode - Air
        else
        {
            state = MovementState.air;
        }
    }
    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 50f, ForceMode.Force);
        }

        // on ground
        else if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
          
        }
        
        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }
    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = (rb.velocity.normalized * moveSpeed)*slopeSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }
    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }
    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
    private void DragControl()
    {
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }
    private void HandleHeadBobAndFootstepSounds()
    {
        if (!grounded || state == MovementState.standing || state == MovementState.air || state == MovementState.crouching) return;
        
        if (state == MovementState.walking)
        {
            timer += Time.deltaTime * walkBobSpeed;
            
            sinFunction = Mathf.Sin(timer);
            playerCam.transform.localPosition = new Vector3(playerCam.transform.localPosition.x,
                defaultYPos + sinFunction * walkBobAmount);

            if (sinState == 1 && hasPlayedWalkingSound == false)
            {
                woodClips.clip = sounds[Random.Range(0, sounds.Length)];
                woodClips.volume = Random.Range(0.6f - volumeChangeMultiplier, 0.6f);
                woodClips.pitch = Random.Range(0.85f - pitchChangeMultiplier, 0.85f + pitchChangeMultiplier);
                woodClips.PlayOneShot(woodClips.clip);
                hasPlayedWalkingSound = true;
            }
            
            if (sinFunction > 0) sinState = 1;
            else if (sinFunction < 0)
            {
                sinState = 0;
                hasPlayedWalkingSound = false;
            }
        }
        
        else if (state == MovementState.sprinting)
        {
            timer += Time.deltaTime * sprintBobSpeed;
            
            sinFunction = Mathf.Sin(timer);
            playerCam.transform.localPosition = new Vector3(playerCam.transform.localPosition.x,
                defaultYPos + sinFunction * sprintBobAmount);

            if (sinState == 1 && hasPlayedWalkingSound == false)
            {
                woodClips.clip = sounds[Random.Range(0, sounds.Length)];
                woodClips.volume = Random.Range(0.6f - volumeChangeMultiplier, 0.6f);
                woodClips.pitch = Random.Range(0.85f - pitchChangeMultiplier, 0.85f + pitchChangeMultiplier);
                woodClips.PlayOneShot(woodClips.clip);
                hasPlayedWalkingSound = true;
            }
            
            if (sinFunction > 0) sinState = 1;
            else if (sinFunction < 0)
            {
                sinState = 0;
                hasPlayedWalkingSound = false;
            }
        }
        else if (state == MovementState.crouchWalking)
        {
            timer += Time.deltaTime * crouchBobSpeed;
            
            sinFunction = Mathf.Sin(timer);
            playerCam.transform.localPosition = new Vector3(playerCam.transform.localPosition.x,
                defaultYPos + sinFunction * crouchBobAmount);

            if (sinState == 1 && hasPlayedWalkingSound == false)
            {
                woodClips.clip = sounds[Random.Range(0, sounds.Length)];
                woodClips.volume = Random.Range(0.6f - volumeChangeMultiplier, 0.6f);
                woodClips.pitch = Random.Range(0.85f - pitchChangeMultiplier, 0.85f + pitchChangeMultiplier);
                woodClips.PlayOneShot(woodClips.clip);
                hasPlayedWalkingSound = true;
            }
            
            if (sinFunction > 0) sinState = 1;
            else if (sinFunction < 0)
            {
                sinState = 0;
                hasPlayedWalkingSound = false;
            }
        }

    }
}
