using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]

public class PlayerController3D : MonoBehaviour
{
    PlayerRaycast playerRaycast;

    [Header("Containers")]
    public Rigidbody rb;                                             // Access the rigidbody to move
    public GameObject cam;                                           // Refference to the camera
    public Transform originalCameraPosition;                         // Camera's default position
    public Transform crouchTarget;                                   // Where the camera goes when the player crouches

    [Header("Values")]
    public float mouseSensitivity = 1;                               // Mouse sensitivity
    public float jumpHeightIntensifier = 1;                          // How far i can jump
    public float playerSpeedIntensifier = 1;                         // We can controll the speed of the player here.
    public float upDownRange = 90.0f;                                // How far i can look up or down.

    public float valOfVelocity;                                      // Checks how fast the player goes
    public float maxVelocity;                                        // The max speed of how fast the player goes
    public float zoomAmount = 40f;                                   // How much the camera zooms in 
    public float focusSmoothing = 4f;

    private float verticalRotation = 0;                              // Contains the MouseYAxis
    private float originalMaxVelocity;                               // Contains the orginal MaxVelocity  
    private float originalFOV;                                       // Contains the original FOV


    void Awake()
    {
        playerRaycast = GetComponent<PlayerRaycast>();
        originalFOV = Camera.main.fieldOfView;
    }
    
    void Update()
    {
        valOfVelocity = rb.velocity.magnitude;
    }

    public void Focus(bool focusIn)
    {
        if (focusIn == true)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoomAmount, Time.deltaTime * focusSmoothing);
        }
        else
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, originalFOV, Time.deltaTime * focusSmoothing);
        }
    }

    public void Sit(bool isSitting)
    {
        if (isSitting == true)
        {
            gameObject.transform.position = new Vector3(playerRaycast.hitObjectTransform().x, playerRaycast.hitObjectTransform().y + 1f, playerRaycast.hitObjectTransform().z);
        }
    }

    public void Crouch(bool isCrouching)
    {
        if (isCrouching == true)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, crouchTarget.transform.position, Time.deltaTime * focusSmoothing);
        }
        else
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, originalCameraPosition.transform.position, Time.deltaTime * focusSmoothing);
        }
    }

    #region Movement Related
    public void PlayerMove(float xAxis, float zAxis)
    {
        // left and right movement
        if (xAxis != 0)
        {
            if (xAxis > 0)
            {
                if (Mathf.Abs(valOfVelocity) <= maxVelocity)
                {
                    rb.AddForce(transform.right * playerSpeedIntensifier, ForceMode.Impulse);
                }
            }

            if (xAxis < 0)
            {
                if (Mathf.Abs(valOfVelocity) <= maxVelocity)
                {
                    rb.AddForce(-transform.right * playerSpeedIntensifier, ForceMode.Impulse);
                }
            }
        }

        // forward and backward movement
        if (zAxis != 0)
        {
            if (zAxis > 0)
            {
                if (Mathf.Abs(valOfVelocity) <= maxVelocity)
                {
                    rb.AddForce(transform.forward * playerSpeedIntensifier, ForceMode.Impulse);
                }
            }

            if (zAxis < 0)
            {
                if (Mathf.Abs(valOfVelocity) <= maxVelocity)
                {
                    rb.AddForce(-transform.forward * playerSpeedIntensifier, ForceMode.Impulse);
                }
            }
        }
    }

    public void Mouselook(float mouseXAxis, float mouseYAxis)
    {
        //Filter Horizontal input
        if (mouseXAxis != 0)
        {
            gameObject.transform.Rotate(new Vector3(0, mouseXAxis, 0));
        }

        //Filter Vertical input
        if (mouseYAxis != 0)
        {
            verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;               //This section pretty much clamps your camera rotation idk why this works
            verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
            Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        }
    }
    #endregion
}
