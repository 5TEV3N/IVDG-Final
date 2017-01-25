using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]

public class PlayerController3D : MonoBehaviour
{
    InputManager3D inputManager;

    [Header("Values")]
    public float mouseSensitivity = 1;                               // Mouse sensitivity
    public float jumpHeightIntensifier = 1;                          // How far i can jump
    public float playerSpeedIntensifier = 1;                         // We can controll the speed of the player here.
    public float upDownRange = 90.0f;                                // How far i can look up or down.

    public float valOfVelocity;                                      // Checks how fast the player goes
    public float maxVelocity;                                        // The max speed of how fast the player goes

    [Header("Containers")]
    public Rigidbody rb;                                             // Access the rigidbody to move
    public Camera cam;                                               // Acess the Camera of the gameobject

    private float verticalRotation = 0;                              // Contains the MouseYAxis
    private float originalMaxVelocity;                               // Contains the orginal MaxVelocity  

    void Awake()
    {
        inputManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InputManager3D>();
    }
    
    void Update()
    {
        valOfVelocity = rb.velocity.magnitude;
    }

    public void PlayerMove(float xAxis, float zAxis)
    {

        if (xAxis != 0)
        {
            if (xAxis > 0)
            {
                if (Mathf.Abs(valOfVelocity) <= maxVelocity)
                {
                    rb.AddForce(transform.right * playerSpeedIntensifier);
                }
            }

            if (xAxis < 0)
            {
                if (Mathf.Abs(valOfVelocity) <= maxVelocity)
                {
                    rb.AddForce(-transform.right * playerSpeedIntensifier)  ;
                }
            }
        }

        if (zAxis != 0)
        {
            if (zAxis > 0)
            {
                if (Mathf.Abs(valOfVelocity) <= maxVelocity)
                {
                    rb.AddForce(transform.forward * playerSpeedIntensifier);
                }
            }

            if (zAxis < 0)
            {
                if (Mathf.Abs(valOfVelocity) <= maxVelocity)
                {
                    rb.AddForce(-transform.forward * playerSpeedIntensifier);
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
            cam.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        }
    }
}
