using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerController3D))]
[RequireComponent(typeof(PlayerRaycast))]
public class InputManager3D : MonoBehaviour
{
    PlayerController3D playerController3D;      
    PlayerRaycast playerRaycast;                
    GameScreenshot gameScreenshot;
    BirdController birdController;
    GameUI gameUi;

    float xAxis = 0;                             // 1 = right, -1 = left
    float zAxis = 0;                             // 1 = front, -1 back
    float mouseXAxis = 0;                        // left or right movement of mouse (camera). Positive numb = right, Negative numb = left
    float mouseYAxis = 0;                        // up or down movement of mouse (camera). Positive numb = up, Negative numb = down.
    bool cameraLock = true;                      // constantly lock the cursor in the center
    bool check;
    int micButtonCheck;
    
    [Header ("Debug")]
    public bool isMicBeingUsed;
    public bool isSitting;
    public int sittingButtonCheck;

    void Awake()
    {
        playerController3D = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController3D>();
        playerRaycast = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRaycast>();
        gameScreenshot = GameObject.FindGameObjectWithTag("UI").GetComponent<GameScreenshot>();
        gameUi = GameObject.FindGameObjectWithTag("UI").GetComponent<GameUI>();

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
        birdController = GameObject.FindGameObjectWithTag("Bird").GetComponent<BirdController>();

        #region Raycast Interaction
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (sittingButtonCheck == 1)
            {
                isSitting = false;
                sittingButtonCheck = 0;
            }

            if (playerRaycast.PlayerInteraction() == true) 
            {
                if (playerRaycast.hitObject().transform.tag == "Sittable")
                {
                    isSitting = true;
                    if (sittingButtonCheck == 0)
                    {
                        sittingButtonCheck++;
                    }
                }
            }
        }

        if (isSitting == true)
        {
            playerController3D.Sit(isSitting);                                       // display the ui icon that you dismount the sittable
        }

        if (isSitting == false)
        {
            playerController3D.Sit(isSitting);
        }

        if (Input.GetMouseButtonDown(1))                                        //RMB
        {
            if (playerRaycast.PlayerInteraction() == true)
            {
                if (playerRaycast.hitObject().transform.tag == "Bird")
                {
                    //StartCoroutine(gameScreenshot.GetSnapshot());
                    //birdController.playerTookPicture = true;
                    //print("Screenshot saved!");
                }
            }
            StartCoroutine(gameScreenshot.GetSnapshot());
            birdController.playerTookPicture = true;
            print("Screenshot saved!");
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            micButtonCheck++;
            if (micButtonCheck == 1)
            {
                isMicBeingUsed = true;
            }
            if (micButtonCheck == 2)
            {
                isMicBeingUsed = false;
                micButtonCheck = 0;
            }
            // Add the microphone checks from the AudioSpectrum
        }

        if (isMicBeingUsed == true)
        {
            gameUi.MicInputUI(isMicBeingUsed);
        }
        else
        {
            gameUi.MicInputUI(isMicBeingUsed);
        }

        #endregion
    }
}
