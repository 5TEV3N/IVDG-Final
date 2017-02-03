using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScreenshotToGallery : MonoBehaviour
{
    //populate the list with how much slots there is
    //get ahold of the slot's RawImage Component
    //add the screenshot saved in GameScreenshot into these Components
    //dynamically scale this?
    public List<RawImage> screenshotSlot = new List<RawImage>();

    private int thumbnailIndex = 0;
    private RawImage newThumbnail;

    public void AddThumbnail(byte[] screenshotBytes)
    {
        if (thumbnailIndex < screenshotSlot.Count)                                                                          //PROBLEM, THE TEXTURE GETS DISTORTED WHEN SAVE, PLEASE FIX LATER
        {
            Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, true);           //make a new texture2d to put into the ui's raw image
            screenshotTexture.LoadRawTextureData(screenshotBytes);                                                          //fills the screenshotTexture with the data with the bytes of when the player first took the screenshot
            screenshotTexture.Apply();                                                                                      //apply the data to the texture

            screenshotSlot[thumbnailIndex].texture = screenshotTexture;                                                     //add the texture into the screenshotSlot into the index number = thumbnailIndex
            thumbnailIndex++;                                                                                               //go to the next itteration
        }

        if (thumbnailIndex == screenshotSlot.Count)
        {
            screenshotSlot.Add(newThumbnail);
            //add a raw Image Gameobject UI into the slot
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