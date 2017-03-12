using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager3D : MonoBehaviour
{
    PlayerController3D playerController3D;      
    PlayerRaycast playerRaycast;                
    GameScreenshot gameScreenshot;
    BirdController birdController;   

    float xAxis = 0;                             // 1 = right, -1 = left
    float zAxis = 0;                             // 1 = front, -1 back
    float mouseXAxis = 0;                        // left or right movement of mouse (camera). Positive numb = right, Negative numb = left
    float mouseYAxis = 0;                        // up or down movement of mouse (camera). Positive numb = up, Negative numb = down.
    bool cameraLock = true;                      // constantly lock the cursor in the center

    void Awake()
    {
        playerController3D = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController3D>();
        playerRaycast = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRaycast>();
        gameScreenshot = GameObject.FindGameObjectWithTag("UI").GetComponent<GameScreenshot>();
        birdController = GameObject.FindGameObjectWithTag("Bird").GetComponent<BirdController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        mouseXAxis = Input.GetAxis("Mouse X");
        mouseYAxis = Input.GetAxis("Mouse Y");

        xAxis = Input.GetAxisRaw("Horizontal");
        zAxis = Input.GetAxisRaw("Vertical");

        #region Character + Camera Movement
        if (mouseXAxis != 0 || mouseYAxis != 0)
        {
            playerController3D.Mouselook(mouseXAxis, mouseYAxis);
        }

        if (xAxis != 0 || zAxis != 0)
        {
            playerController3D.PlayerMove(xAxis, zAxis);
        }
        #endregion
    }

    void Update()
    {
        #region Raycast Interaction
        if (Input.GetMouseButton(0))                                            //LMB
        {
            if (playerRaycast.PlayerInteraction() == true)
            {
                if (playerRaycast.hitObject().transform.tag == "Sittable")
                {
                    playerController3D.Sit();
                }
            }
        }
        if (Input.GetMouseButtonDown(1))                                        //RMB
        {
            if (playerRaycast.PlayerInteraction() == true)
            {
                if (playerRaycast.hitObject().transform.tag == "Bird")
                {
                    StartCoroutine(gameScreenshot.GetSnapshot());
                    birdController.playerTookPicture = true;
                    print("Screenshot saved!");
                }
            }
        }
        #endregion

        #region Camera + UI
        if (Input.GetKey(KeyCode.F))
        {
            playerController3D.Focus(true);
        }

        if (!Input.GetKey(KeyCode.F))
        {
            playerController3D.Focus(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cameraLock = true;

            if (cameraLock == true)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                GameUI.gameUi.gamePause.SetActive(true);

                cameraLock = false;
                Time.timeScale = 0f;
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (gameScreenshot.isScreenshotMenuOpen == false)
            {
                gameScreenshot.screenshotMenu.SetActive(true);
                gameScreenshot.isScreenshotMenuOpen = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f;
            }

            else
            {
                gameScreenshot.screenshotMenu.SetActive(false);
                gameScreenshot.isScreenshotMenuOpen = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1f;
            }
        }
        #endregion
    }
}
