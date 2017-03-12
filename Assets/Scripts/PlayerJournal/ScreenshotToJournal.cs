using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotToJournal: MonoBehaviour
{
    BirdInfoToJournal info;
    [Space(10)]
    public GameObject newJournalPage;
    public Transform screenshotParentTransform;
    public int PageIndex;

    [Header("Lists")]
    public List<RawImage> screenshotSlot = new List<RawImage>();
    public List<GameObject> newPagesList = new List<GameObject>();

    private Texture2D screenshotTexture;
    private RawImage[] newPageSlotsComponents;
    private int screenshotPageNumber;
    private int slotIndex;

    void Awake()
    {
        info = GetComponent<BirdInfoToJournal>();
    }

    public void AddThumbnail(byte[] screenshotBytes)
    {
        info.AddInfo();
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
    }

    public void NextScreenshotPage()
    {
        if (PageIndex + 1 < newPagesList.Count)
        {
            PageIndex++;
            newPagesList[PageIndex].SetActive(true);
            info.newBirdInfoPageList[PageIndex].SetActive(true);
            info.newInputFieldsList[PageIndex].SetActive(true);
        }
        else { print("Debug: No pages to go forward to"); }
    }

    public void PreviousScreenshotPage()
    {
        if (PageIndex > 0)
        {
            if (PageIndex - 1 < newPagesList.Count)
            {
                newPagesList[PageIndex].SetActive(false);
                info.newBirdInfoPageList[PageIndex].SetActive(false);
                info.newInputFieldsList[PageIndex].SetActive(false);
                PageIndex--;
            }
        }
        else { print("Debug: No pages to go back to"); }
    }
}

/* REFFERENCE
 * http://gamedev.stackexchange.com/questions/92257/loading-png-file-and-using-it-for-unityengine-ui-image
 * http://answers.unity3d.com/questions/710833/using-getcomponent-with-an-array.html
 * http://answers.unity3d.com/questions/963675/how-to-create-an-image-gallery-with-previous-and-n.html
 * carmack!
 * 
 * Side note: ternary operator
 * thumbnail.texture = (thumbnail.texture == null) ? screenshot : null;
 * variable = (ifConditionTrue)? TrueValue : FalseValue;
 */
