using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotToJournal: MonoBehaviour
{
    BirdInfoManager addInfo;
    [Space(10)]
    public GameObject newJournalPage;
    public Transform screenshotParentTransform;
    public List<RawImage> screenshotSlot = new List<RawImage>();
    public List<GameObject> newPagesList = new List<GameObject>();
    public static int journalIndex;
    
    private Texture2D screenshotTexture;
    private RawImage[] newPageSlotsComponents;
    private int screenshotPageNumber;
    private int screenshotPageIndex;

    void Awake()
    {
        addInfo = GetComponent<BirdInfoManager>();
    }

    public void AddThumbnail(byte[] screenshotBytes)
    {
        if (journalIndex < screenshotSlot.Count)
        {
            screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, true);                     //make a new texture2d to put into the ui's raw image
            screenshotTexture.LoadRawTextureData(screenshotBytes);                                                          //fills the screenshotTexture with the data with the bytes of when the player first took the screenshot
            screenshotTexture.Apply();                                                                                      //apply the data to the texture

            screenshotSlot[journalIndex].texture = screenshotTexture;                                                       //add the texture into the screenshotSlot into the index number = thumbnailIndex
            //addInfo.AddName();
            journalIndex++;                                                                                                 //go to the next itteration
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
        if (screenshotPageIndex + 1 < newPagesList.Count)
        {
            screenshotPageIndex++;
            newPagesList[screenshotPageIndex].SetActive(true);
        }
        else { print("Debug: No pages to go forward to"); }
    }

    public void PreviousScreenshotPage()
    {
        if (screenshotPageIndex > 0)
        {
            if (screenshotPageIndex - 1 < newPagesList.Count)
            {
                newPagesList[screenshotPageIndex].SetActive(false);
                screenshotPageIndex--;
            }
        }
        else { print("Debug: No pages to go back to"); }
    }
}

// REFFERENCE
//http://gamedev.stackexchange.com/questions/92257/loading-png-file-and-using-it-for-unityengine-ui-image
//http://answers.unity3d.com/questions/710833/using-getcomponent-with-an-array.html
//http://answers.unity3d.com/questions/963675/how-to-create-an-image-gallery-with-previous-and-n.html
//carmack!

//Side note: ternary operator
//thumbnail.texture = (thumbnail.texture == null) ? screenshot : null;
//variable = (ifConditionTrue)? TrueValue : FalseValue;