using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScreenshotToGallery : MonoBehaviour
{
    //Dynamically scale this
    public int thumbnailIndex = 0;
    public List<RawImage> screenshotSlot = new List<RawImage>();
    public RawImage newThumbnail;
    public GameObject screenshotPage;
    public Transform canvasTransform;

    private int pageCounter = 1;
    private GameObject newScreenshotPage;
    private RawImage newScreenshotPageComponents;

    void Awake()
    {
        newScreenshotPageComponents = screenshotPage.GetComponentInChildren<RawImage>();
    }

    public void AddThumbnail(byte[] screenshotBytes)
    {
        if (thumbnailIndex == screenshotSlot.Count)
        {
            GameObject newScreenshotPage = Instantiate(screenshotPage, canvasTransform);
            newScreenshotPage.name = "ScreenshotPage " + ++pageCounter;
            //incorpriate the insantiated screenshotPage here
            screenshotSlot.Add(newScreenshotPageComponents);
            thumbnailIndex = 0;
            //make it so that index is not the same number as the screenshotslot.count
        }

        if (thumbnailIndex < screenshotSlot.Count)                                                                          
        {
            Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, true);           //make a new texture2d to put into the ui's raw image
            screenshotTexture.LoadRawTextureData(screenshotBytes);                                                          //fills the screenshotTexture with the data with the bytes of when the player first took the screenshot
            screenshotTexture.Apply();                                                                                      //apply the data to the texture

            screenshotSlot[thumbnailIndex].texture = screenshotTexture;                                                     //add the texture into the screenshotSlot into the index number = thumbnailIndex
            thumbnailIndex++;                                                                                               //go to the next itteration
        }
    }
}

// REFFERENCE
//http://gamedev.stackexchange.com/questions/92257/loading-png-file-and-using-it-for-unityengine-ui-image
//http://answers.unity3d.com/questions/710833/using-getcomponent-with-an-array.html
//carmack!

//ternary operator
//thumbnail.texture = (thumbnail.texture == null) ? screenshot : null;
//variable = (ifConditionTrue)? TrueValue : FalseValue;