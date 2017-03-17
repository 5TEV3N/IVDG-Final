using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotToJournal: MonoBehaviour
{
    BirdInfoToJournal info;

    [Header ("Containers")]
    public GameObject newPicture;
    public Transform screenshotParentTransform;
    public Texture2D screenshotTexture;

    [Header ("Values")]
    public int journalPageIndex;
    public int inverseJournalPageIndex;
    public int slotIndex;
    private int screenshotPageNumber;

    [Header("Lists")]
    public List<RawImage> screenshotSlot = new List<RawImage>();
    public List<GameObject> newPagesList = new List<GameObject>();
    public RawImage[] newPageSlotsComponents;

    void Awake()
    {
        info = GetComponent<BirdInfoToJournal>();
    }

    public void AddThumbnail(byte[] screenshotBytes)
    {//Side note: Please make it so that players won't be able to see the screenshot page unless they took one
        info.AddInfo();
		info.AddInputField();
        if (slotIndex < screenshotSlot.Count)
        {
            // Bootstrap, Applies the texture2d into the raw image
            screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, true);
            screenshotTexture.LoadRawTextureData(screenshotBytes);
            screenshotTexture.Apply();
            screenshotSlot[slotIndex].texture = screenshotTexture;
            slotIndex++;
        }
        else
        {
            // Makes a new page
            newPicture = Instantiate(newPicture, screenshotParentTransform, false);
            newPicture.name = "RawImageSlot" + ++screenshotPageNumber;
            newPicture.SetActive(false);
            // Method of extending the screenshotSlot index. Adds new raw image into screenshot slot index, making it ready so that the player can view the next screenshot
            newPageSlotsComponents = newPicture.GetComponents<RawImage>();
            screenshotSlot.AddRange(newPageSlotsComponents);
            newPagesList.Add(newPicture);
            // Applies the screenshot
            screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, true);
            screenshotTexture.LoadRawTextureData(screenshotBytes);
            screenshotTexture.Apply();
            screenshotSlot[slotIndex].texture = screenshotTexture;
            slotIndex++;
        }
    }

    public void NextJournalPage()
    {
        if (journalPageIndex + 1 < newPagesList.Count)
        {
            journalPageIndex++;

			newPagesList[journalPageIndex].SetActive(true);
            info.newBirdNamePageList[journalPageIndex].SetActive(true);
			info.newInputFieldsList[journalPageIndex].SetActive(true);        
		}
        else { print("Debug: No pages to go forward to"); }
    }

    public void PreviousJournalPage()
    {
        if (journalPageIndex > 0)
        {
            if (journalPageIndex - 1 < newPagesList.Count)
            {
                newPagesList[journalPageIndex].SetActive(false);
                info.newBirdNamePageList[journalPageIndex].SetActive(false);
				info.newInputFieldsList [journalPageIndex].SetActive (false);       
				journalPageIndex--;
            }
        }
        else { print("Debug: No pages to go back to"); }
    }
}

/* REFFERENCE
 * http://gamedev.stackexchange.com/questions/92257/loading-png-file-and-using-it-for-unityengine-ui-image
 * http://answers.unity3d.com/questions/710833/using-getcomponent-with-an-array.html
 * http://answers.unity3d.com/questions/963675/how-to-create-an-image-gallery-with-previous-and-n.html
 * Kaermack!
 * 
 * Side note: ternary operator
 * thumbnail.texture = (thumbnail.texture == null) ? screenshot : null;
 * variable = (ifConditionTrue)? TrueValue : FalseValue;
 */

/*
 * info.AddInfo();
       if (slotIndex < screenshotSlot.Count)
       {
           screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, true);                     //make a new texture2d to put into the ui's raw image
           screenshotTexture.LoadRawTextureData(screenshotBytes);                                                          //fills the screenshotTexture with the data with the bytes of when the player first took the screenshot
           screenshotTexture.Apply();                                                                                      //apply the data to the texture
           screenshotSlot[slotIndex].texture = screenshotTexture;                                                          //add the texture into the screenshotSlot into the index number = thumbnailIndex
           slotIndex++;                                                                                                    //go to the next itteration
       }
       else
       {
           newJournalPage = Instantiate(newJournalPage, screenshotParentTransform, false);                                 //instantiate a new page
           newJournalPage.name = "JournalPage" + ++screenshotPageNumber;
           newJournalPage.SetActive(false);

           for (int i = 0; i < newJournalPage.transform.childCount; i++)                                                   //gets the components inside of the pages
           {
               newPageSlotsComponents = newJournalPage.GetComponentsInChildren<RawImage>();
           }
           screenshotSlot.AddRange(newPageSlotsComponents);                                                                //add those components into the new pages
           newPagesList.Add(newJournalPage);
       }
*/
