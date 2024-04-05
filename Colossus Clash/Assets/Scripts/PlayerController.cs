using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Controls controls;
    private Vector2 moveVector;
    private Vector2 lookVector;
    private Vector2 lastLookVector;
    private Vector3 mousePos;

    /// <summary>
    /// So for those who may be unfamiliar, Serialized fields are what
    /// we do keep PRIVATE variables, but still be able to edit them in Unity's inspector
    /// -Luke
    /// </summary> test
    /// 

    //Get references to each control set
    [SerializeField] private InputActionReference shoot, roll, switchWeapons;

    //Speed of Player (Edit in hierarchy, changing numbers here doesn't do anything)
    //[SerializeField] private float speed = 5.0f;
    
    //Set Deadzones (Edit in hierarchy, changing numbers here doesn't do anything)
    [SerializeField] private float leftXDeadZ = 0.1f;
    [SerializeField] private float leftYDeadZ = 0.1f;
    [SerializeField] private float rightXDeadZ = 0.1f;
    [SerializeField] private float rightYDeadZ = 0.1f;

    private float t = 0.0f; //Used for LERPING when rotating. Think of it like "rotationLerpTimeRatio"
    private bool lerping; //Are we lerping?
    [SerializeField] private float turnTime = 1f;
    private bool activeStickR; //Is the stick (right) is being touched?

    //movement and dash variables  ***
    [SerializeField] private float MAX_SPEED = 10.0f; 
    private float currentSpeed = 0.0f; // Unserialized current speed float
    private bool isDashing = false; // To check if the player is dashing
    private float dashSpeedMultiplier = 2.0f; // Speed multiplier for dash
    private float dashDuration = 0.5f; // How long the dash effect lasts
    private float dashTimer = 0.0f; // Timer to track dash duration
    [SerializeField] private float accelerationTime = 5.0f; // Time it takes to reach max speed
    private float speedPercent = 0.0f; // Current speed as a percentage of max speed
    //*****

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Start");
        controls = new Controls(); //Turned on controls
        controls.PlayerMap.Enable(); //Enabled player object
    }

    // Update is called once per frame
    void Update()
    {
        ReadStickInput();

        Move();

        Look();

        if (isDashing)
        {
            Dash();
        }
 
    }

    //On enable, connect each function to controls
    private void OnEnable()
    {
        shoot.action.performed += Shoot;
        roll.action.performed += DashInitiate;
        switchWeapons.action.performed += SwitchWeapons;
    }

    //On disable, disconnect all functions
    private void OnDisable()
    {
        shoot.action.performed -= Shoot;
        roll.action.performed -= DashInitiate;
        switchWeapons.action.performed -= SwitchWeapons;
    }

    public void Shoot(InputAction.CallbackContext context) //Need special parameter called Callback context. We receive this from Unity's Input system
    {
        UnityEngine.Debug.Log("Bang!");
    }

    public void DashInitiate(InputAction.CallbackContext context)
    {
        if (!isDashing) // Start dashing if not already dashing
        {
            isDashing = true;
            dashTimer = dashDuration;
            UnityEngine.Debug.Log("Dash!");
        }
    }

    private void Dash()
    {
        dashTimer -= Time.deltaTime;
        if (dashTimer <= 0)
        {
            isDashing = false;
        }
    }

    //This does nothing functionally right now, but the keybind and debug log works
    public void Roll(InputAction.CallbackContext context)
    {
        UnityEngine.Debug.Log("Roll!");
    }
    
    //This does nothing functionally right now, but the keybind and debug log works
    public void SwitchWeapons(InputAction.CallbackContext context)
    {
        UnityEngine.Debug.Log("Switched!");
    }
    
    public void OnRotate(InputAction.CallbackContext value)
    {
        Vector2 direction = value.ReadValue<Vector2>(); //Create a new Vec2 with the input direction
        var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y).normalized, Vector3.up); //Create a normalized quaternion
        this.transform.rotation = lookRotation; //Set our rotation to that new quaternion that was based on input direction
    }

    private void ReadStickInput()
    {
        //Read vectors every frame
        moveVector = controls.PlayerMap.MoveControl.ReadValue<Vector2>();
        lookVector = controls.PlayerMap.LookControl.ReadValue<Vector2>();
        activeStickR = (Mathf.Abs(lookVector.x) > rightXDeadZ || Mathf.Abs(lookVector.y) > rightYDeadZ);
    }

    private void Move()
    {
        if (Mathf.Abs(moveVector.x) > leftXDeadZ || Mathf.Abs(moveVector.y) > leftYDeadZ) //WAS moveVector != Vector2.zero, but making "Deadzones"
        {
            if (!isDashing)
            {
                // Smooth acceleration to max speed over inputted (in editor) seconds
                speedPercent += Time.deltaTime / accelerationTime;
                speedPercent = Mathf.Clamp01(speedPercent);
                currentSpeed = speedPercent * MAX_SPEED;
            }
            else
            {
                currentSpeed = MAX_SPEED * dashSpeedMultiplier; // Apply dash speed multiplier
            }
            Vector3 moveDirection = new Vector3(moveVector.x, moveVector.y, 0).normalized;
            this.transform.position += moveDirection * (currentSpeed * Time.deltaTime);
        }
        else
        {
            speedPercent = 0; // Reset speed percent if not moving
        }
    }


    /// <summary>
    /// A littl context here. This is Liner Interpolation, I added it into our Look method
    /// Because if you snap the stick in the opposite direction, it sets the rotation to the other side,
    /// it doesn't rotate. So this LERPs between the two points in something like, 5 frames? Depends on
    /// the computer but it feels MUCH better.
    /// </summary>
    private void Look()
    {
        //All hail the LERP. This shit is crazy.
        LookLerpTimer();

        //Set players look vector after LERPING
        //(Mathf.Abs(lookVector.x) > rightXDeadZ || Mathf.Abs(lookVector.y) > rightYDeadZ)
        if (activeStickR || lerping)
        {
            //Debug.Log("Right Stick: " + lookVector);
            lerping = true; //We are moving, so lerp
            float currentHeading = Mathf.Atan2(-lookVector.x, lookVector.y); // Gets where we are currently looking for LERP
            float lastFrameHeading = Mathf.Atan2(-lastLookVector.x, lastLookVector.y); //Gets last frames look vector for LERP

            //LERP TIME
            float heading = Mathf.Lerp(lastFrameHeading, currentHeading, t);

            if (activeStickR)
            {

                /* Down below is logic for rotating with the mouse, basically the mousePos needs to be in world space not screenspace
                Input.mousePosition is in screenspace so I use Camera.main.ScreenToWorldPoint to convert it, then just get a direction 
                by subtracting transform.position from mousePos and set the rotation in that direction.

                I also added lastMousePos to check if the mouse actually moved, which should not happen if the player is using controller
                that way this code will use the correct logic depending on which control method is being used this does not really work, 
                it's whats making the mouse rotation kinda spastic so i'll figure out a better way later - Paul */

                Vector3 lastMousePos = mousePos;
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (lastMousePos != mousePos)
                {
                    Vector3 direction = mousePos - transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
                    transform.rotation = targetRotation;
                }
            }
            //Debug.Log(heading);
        }
        lastLookVector = controls.PlayerMap.LookControl.ReadValue<Vector2>();; //Assign at the end of update, this gets the same value as lookVector
    }

    private void LookLerpTimer()
    {
        if (lerping) //Check to see if we are rotating
        {
            t += turnTime * Time.deltaTime; //Add to T for Lerp
        
            //Check if t needs to be reset
            if (t >= 1f)
            {
                t = 0.0f;
                lerping = false;
            }
        }
    }
    
}
