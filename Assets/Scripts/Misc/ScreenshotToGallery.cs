using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScreenshotToGallery : MonoBehaviour
{
    // populate the list with how much slots there is
    // get ahold of the slot's RawImage Component
    // add the screenshot saved in GameScreenshot into these Components
    // dynamically scale this

    //public List<RawImage> screenshotSlot= new List<RawImage>();
    public RawImage[] screenshotSlot;
    private int thumbnailIndex = 0;

    public void AddThumbnail(byte[] screenshotBytes)
    {
        if (thumbnailIndex < screenshotSlot.Length)                                                                         //
        {
            Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24,false);            //make a new texture2d to put into the ui's raw image
            screenshotTexture.LoadRawTextureData(screenshotBytes);                                                          //fills the screenshotTexture with the data with the bytes of when the player first took the screenshot
            screenshotTexture.Apply();                                                                                      //apply the data to the texture

            screenshotSlot[thumbnailIndex].texture = screenshotTexture;                                                     //
            thumbnailIndex++;                                                                                               //
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