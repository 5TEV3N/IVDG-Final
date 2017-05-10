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
    FootstepsLoops footstepsLoops;
    [Header("Interaction Keys")]
    //Default Keys
    public KeyCode snapshotCaptureKey = KeyCode.Mouse1;
    public KeyCode cameraKey = KeyCode.Space;
    public KeyCode playerJournalKey = KeyCode.Tab;
    public KeyCode interactionKey = KeyCode.E;
    public KeyCode focusKey = KeyCode.F;
    public KeyCode crouchKey = KeyCode.C;

    [Header ("Debug")]
    public bool isMicBeingUsed;
    public bool isSitting;
    public bool isToggleCrouch;
    public bool isToggleFocus;

    private float xAxis = 0;                             // 1 = right, -1 = left
    private float zAxis = 0;                             // 1 = front, -1 back
    private float mouseXAxis = 0;                        // left or right movement of mouse (camera). Positive numb = right, Negative numb = left
    private float mouseYAxis = 0;                        // up or down movement of mouse (camera). Positive numb = up, Negative numb = down.
    private bool cameraLock = true;                      // constantly lock the cursor in the centers
    private bool crouchCheck;
    private bool focusCheck;
    private int crouchButtonCheck = 0;
    private int focusButtonCheck = 0;
    private int sittingButtonCheck = 0;
    private int cameraButtonCheck = 0;
    private bool footstepsStarted = false;

    void Awake()
    {
        playerController3D = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController3D>();
        playerRaycast = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRaycast>();
        gameScreenshot = GameObject.FindGameObjectWithTag("UI").GetComponent<GameScreenshot>();
        gameUi = GameObject.FindGameObjectWithTag("UI").GetComponent<GameUI>();
        footstepsLoops = GameObject.Find("Footsteps").GetComponent<FootstepsLoops>();

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
            PlayFootsteps(true);
        }
        else { PlayFootsteps(false); }
        #endregion
    }
    void Update()
    {
        birdController = GameObject.FindGameObjectWithTag("Bird").GetComponent<BirdController>();

        #region Interactions

        #region >InteractionKey
        if (Input.GetKeyDown(interactionKey))
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
            playerController3D.Sit(isSitting);                                  // display the ui icon that you dismount the sittable
        }

        if (isSitting == false)
        {
            playerController3D.Sit(isSitting);
        }
        #endregion

        #region >CameraKeys
        if (Input.GetKeyDown(cameraKey))
        {
            cameraButtonCheck++;
        }
        if (cameraButtonCheck == 1)
        {
            gameUi.cameraScreen.SetActive(true);
        }
        if (cameraButtonCheck == 2)
        {
            gameUi.cameraScreen.SetActive(false);
            cameraButtonCheck = 0;
        }

        if (Input.GetKeyDown(snapshotCaptureKey))                                        //RMB
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

        #region >CrouchKey

        if (isToggleCrouch == false)
        {
            if (Input.GetKey(crouchKey))
            {
                crouchCheck = true;
                playerController3D.Crouch(crouchCheck);
            }

            if (!Input.GetKey(crouchKey))
            {
                crouchCheck = false;
                playerController3D.Crouch(crouchCheck);
            }
        }

        if (isToggleCrouch == true)
        {
            if (Input.GetKeyDown(crouchKey))
            {
                crouchButtonCheck++;
            }

            if (crouchButtonCheck == 1)
            {
                crouchCheck = true;
            }

            if (crouchButtonCheck == 2)
            {
                crouchCheck = false;
                crouchButtonCheck = 0;
            }

            if (crouchCheck == true)
            {
                playerController3D.Crouch(true);
            }

            if (crouchCheck == false)
            {
                playerController3D.Crouch(false);
            }
        }
        #endregion

        #endregion

        #region Camera + UI

        #region >FocusKey
        if (isToggleFocus == false)
        {
            if (Input.GetKey(focusKey))
            {
                playerController3D.Focus(true);
            }

            if (!Input.GetKey(focusKey))
            {
                playerController3D.Focus(false);
            }
        }

        if (isToggleFocus == true)
        {
            if (Input.GetKeyDown(focusKey))
            {
                focusButtonCheck++;
            }

            if (focusButtonCheck == 0)
            {
                playerController3D.Focus(false);
            }

            if (focusButtonCheck == 1)
            {
                playerController3D.Focus(true);
            }

            if (focusButtonCheck == 2)
            {
                focusButtonCheck = 0;
            }
        }

        #endregion

        #region >MenuScreen
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
        #endregion

        #region >PlayerJournal
        if (Input.GetKeyDown(playerJournalKey))
        {
            if (gameScreenshot.screenshotTook == true)
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
            else { Debug.Log("Debug: Can't open journal if there's nothing inside of it."); }
        }
        #endregion

        #endregion
    }
    public void PlayFootsteps(bool isWalking)
    {   
        if (isWalking == true)
        {
            if (footstepsStarted == false)
            {
                footstepsLoops.FootstepsStart();
                footstepsStarted = true;
            }
        }
        if (isWalking == false)
        {
            footstepsLoops.FootstepsStop();
            footstepsStarted = false;
        }
    }
}
