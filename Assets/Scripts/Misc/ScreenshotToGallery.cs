using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScreenshotToGallery : MonoBehaviour
{
    //Dynamically scale this
    public int thumbnailIndex = 0;
    public List<RawImage> screenshotSlot = new List<RawImage>();
    public Texture2D screenshotTexture;
    public GameObject screenshotPage;
    public Transform screenshotParentTransform;
    private RawImage[] newPageSlotsComponents;

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
            screenshotPage = Instantiate(screenshotPage,screenshotParentTransform, false);                                  //instantiate a new page
            for (int i = 0; i < screenshotPage.transform.childCount; i++)                                                   //gets the components inside of the pages
            {
                newPageSlotsComponents = screenshotPage.GetComponentsInChildren<RawImage>();
            }
            screenshotSlot.AddRange(newPageSlotsComponents);                                                                //add those components into the new pages
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