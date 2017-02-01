using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// PLACE THIS ON THE SCREENSHOT GALLERY UI GAMEOBJECT 

public class ScreenshotToGallery : MonoBehaviour
{
    // populate the list with how much slots there is
    // get ahold of the slot's RawImage Component
    // add the screenshot saved in GameScreenshot into these Components
    // dynamically scale this

    //public List<RawImage> screenshotSlot= new List<RawImage>();
    public RawImage[] screenshotSlot;

    private RawImage uiRawImage;
    private Texture2D snapshotThumbnail;


    void Update()
    {
        for (int i = 0; i< screenshotSlot.Length; i++)
        {
            
        }

        foreach (RawImage imagepanel in screenshotSlot)
        {
            //Get the RawImage Components here
            //override the RawImage's Texture to the GameScreenshot.screenshotGallery 's Texture
        }
    }
}
// REFFERENCE
//http://gamedev.stackexchange.com/questions/92257/loading-png-file-and-using-it-for-unityengine-ui-image
//http://answers.unity3d.com/questions/710833/using-getcomponent-with-an-array.html

/* randomscrap?
uiRawImage = gameObject.GetComponent<RawImage>();  //RawImage

snapshotThumbnail = GameScreenshot.screenShot;     //Texture2D
uiRawImage.texture = snapshotThumbnail;            //RawImage
*/
