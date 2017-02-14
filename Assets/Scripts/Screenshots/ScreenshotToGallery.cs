using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotToGallery : MonoBehaviour
{
    public List<RawImage> screenshotSlot = new List<RawImage>();
    public Texture2D screenshotTexture;
    public GameObject newScreenshotPage;
    public Transform screenshotParentTransform;

    private RawImage[] newPageSlotsComponents;
    private int thumbnailIndex = 0;

    private static int screenshotPageCounter = 1;

    public void AddThumbnail(byte[] screenshotBytes)
    {
        if (thumbnailIndex < screenshotSlot.Count)
        {
            screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, true);                     //make a new texture2d to put into the ui's raw image
            screenshotTexture.LoadRawTextureData(screenshotBytes);                                                          //fills the screenshotTexture with the data with the bytes of when the player first took the screenshot
            screenshotTexture.Apply();                                                                                      //apply the data to the texture

            screenshotSlot[thumbnailIndex].texture = screenshotTexture;                                                     //add the texture into the screenshotSlot into the index number = thumbnailIndex
            thumbnailIndex++;                                                                                               //go to the next itteration
        }
        else
        {
            newScreenshotPage = Instantiate(newScreenshotPage, screenshotParentTransform, false);                           //instantiate a new page
            newScreenshotPage.name = "ScreenshotPage" + ++screenshotPageCounter;

            for (int i = 0; i < newScreenshotPage.transform.childCount; i++)                                                //gets the components inside of the pages
            {
                newPageSlotsComponents = newScreenshotPage.GetComponentsInChildren<RawImage>();
            }
            screenshotSlot.AddRange(newPageSlotsComponents);                                                                //add those components into the new pages
        }
    }

    public void NextScreenshotPage()
    {

    }

    public void PreviousScreenshotPage()
    {

    }

}

// REFFERENCE
//http://gamedev.stackexchange.com/questions/92257/loading-png-file-and-using-it-for-unityengine-ui-image
//http://answers.unity3d.com/questions/710833/using-getcomponent-with-an-array.html
//carmack!

//ternary operator
//thumbnail.texture = (thumbnail.texture == null) ? screenshot : null;
//variable = (ifConditionTrue)? TrueValue : FalseValue;