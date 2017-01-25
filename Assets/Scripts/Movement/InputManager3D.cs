using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager3D : MonoBehaviour
{
    PlayerController3D playerController3D;       // refference to the playerController3D script

    public Sprite playerSprite;                  // sprite of the player character

    float xAxis = 0;                             // 1 = right, -1 = left
    float zAxis = 0;                             // 1 = front, -1 back
    float mouseXAxis = 0;                        // left or right movement of mouse (camera). Positive numb = right, Negative numb = left
    float mouseYAxis = 0;                        // up or down movement of mouse (camera). Positive numb = up, Negative numb = down.
    bool cameraLock = true;                      // constantly lock the cursor in the center

    void Awake()
    {
        playerController3D = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController3D>();
    }

    void FixedUpdate()
    { 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mouseXAxis = Input.GetAxis("Mouse X");
        mouseYAxis = Input.GetAxis("Mouse Y");

        xAxis = Input.GetAxisRaw("Horizontal");
        zAxis = Input.GetAxisRaw("Vertical");

        if (mouseXAxis != 0 || mouseYAxis != 0)
        {
            playerController3D.Mouselook(mouseXAxis, mouseYAxis);
        }

        if (xAxis != 0 || zAxis != 0)
        {
            playerController3D.PlayerMove(xAxis, zAxis);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cameraLock = true;

            if (cameraLock == true)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                cameraLock = false;
            }
        }
    }
}
