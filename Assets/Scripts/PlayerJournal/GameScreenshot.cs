using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

[RequireComponent(typeof (ScreenshotToJournal))]

public class GameScreenshot : MonoBehaviour
{
    PlayerRaycast playerRaycast;
    BirdState birdState;
    GameUI gameUI;
    InputManager3D inputManager;

    [Header ("Containers")]
    public GameObject screenshotMenu;                                                                       //Container for the screenshot menu. InputManager access this
    private Texture2D screenShot;

    [Header("Logic Checks")]
    public bool isScreenshotMenuOpen = false;
    public bool screenshotTook = false;

    void Awake()
    {
        gameUI = GetComponent<GameUI>();
        screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, true);
        inputManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InputManager3D>();
    }

    public IEnumerator GetSnapshot()                                                                        //the act of taking a screenshot
    {
        screenshotTook = true;
        gameUI.cameraScreen.SetActive(false);
        gameUI.cameraIcon.SetActive(false);
        gameUI.journalIcon.SetActive(false);
        gameUI.cameraTutorialText.gameObject.SetActive(false);
        inputManager.cameraButtonCheck = 0;

        yield return new WaitForEndOfFrame();                                                               //apparently you need wait for the end of the frame or else you get some sort of error
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);                     //something about reading the texture from the screen into the saved texture data
        screenShot.Apply();                                                                                 //apply the texture into screenShot
        gameUI.CameraFlash(true);
        
        GetComponent<ScreenshotToJournal>().AddThumbnail(screenShot.GetRawTextureData());                   //get the ScreenshotToGallery component and get the addthumbnail function and feed it the screenshot's raw texture data
        yield return new WaitForSeconds(1);
        gameUI.cameraIcon.SetActive(true);
        gameUI.journalIcon.SetActive(true);
        gameUI.CameraFlash(false);
    }
}

/*  REFFERENCE
 * http://answers.unity3d.com/questions/393431/capturing-screen-shot-and-showing-the-captured-ima.html
 * https://aarlangdi.blogspot.ca/2016/07/saving-screen-shot-in-unity-3d.html
 */
/* For saving it into a picture format:

       byte[] bytes = screenShot.EncodeToJPG();                                                            //encodes the the texture 2d into png
       screenshotName = "/Screenshot" + ++screenshotNumber + ".jpg";                                       //the naming convention for the screenshot
       File.WriteAllBytes(Application.dataPath + screenshotName, bytes);                                   //this is where it saves the screenshot??? 
       screenshotTook = true;                                                                              //show the screenshot in the GUI    

       yield return new WaitForSeconds(4f);
       screenshotTook = false;                                                                             //turn it off
*/
