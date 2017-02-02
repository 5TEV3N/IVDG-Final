using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
[RequireComponent(typeof (ScreenshotToGallery))]

public class GameScreenshot: MonoBehaviour
{
    public static Texture2D screenShot;
    private string screenshotName;
    private bool ifScreenshotTook;

    void Start()
    {
        screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);                //screenshot res
    }

    private void OnGUI()
    {
        if (ifScreenshotTook == true)
        {
            GUI.DrawTexture(new Rect(10, 10, 60, 40), screenShot, ScaleMode.StretchToFill);                 //preview of screenshot
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.End))                                                                  //FOR DEBUGING PURPOSE, CHANGE THIS LATER
        {
            StartCoroutine("GetSnapshot");
        }
    }

    IEnumerator GetSnapshot()                                                                               //the act of taking a screenshot
    {
        yield return new WaitForEndOfFrame();                                                               //apparently you need wait for the end of the frame or else you get some sort of error
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);                           //something about reading the texture from the screen into the saved texture data
        screenShot.Apply();                                                                                 //apply the texture into screenShot

        GetComponent<ScreenshotToGallery>().AddThumbnail(screenShot.GetRawTextureData());                   //get the ScreenshotToGallery component and get the addthumbnail function and feed it the screenshot's raw texture data
                                                                                                            //somehow, save the screenshot list so that player always haves in when they execute the game

        byte[] bytes = screenShot.EncodeToPNG();                                                            //encodes the the texture 2d into png
        screenshotName = "/screenshot" + Time.time + ".png";                                                //the naming convention for the screenshot
        File.WriteAllBytes(Application.dataPath + screenshotName, bytes);                                   //this is where it saves the screenshot??? 
        ifScreenshotTook = true;                                                                            //show the screenshot in the GUI    

        yield return new WaitForSeconds(4f);
        ifScreenshotTook = false;                                                                           //turn it off
    }
}

// REFFERENCE
//http://answers.unity3d.com/questions/393431/capturing-screen-shot-and-showing-the-captured-ima.html
//https://aarlangdi.blogspot.ca/2016/07/saving-screen-shot-in-unity-3d.html