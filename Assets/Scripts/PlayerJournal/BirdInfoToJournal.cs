using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdInfoToJournal : MonoBehaviour
{
    // NOT THE BEST WAY TO GO ABOUT THIS. PLEASE FIGURE OUT A MORE OPTIMAL WAY OF GOING ABOUT THIS LATER
    BirdController currentBirdName;

    public GameObject newBirdInfoPage;
    public Transform birdNameTransform;
    public List<Text> birdNames = new List<Text>();
    public List<GameObject> newBirdInfoPageList = new List<GameObject>();

    private Text[] newTextComponents;
    private int birdJournalPageNumber;
    public int birdInfoIndex;
    private int birdInfoSlot;

    public void AddName()
    {
        currentBirdName = GameObject.Find("Bird").GetComponent<BirdController>();
        if (birdInfoSlot < birdNames.Count)
        {
            birdNames[birdInfoSlot].text = currentBirdName.birdName;
            birdInfoSlot++;
        }
        else
        {
            newBirdInfoPage = Instantiate(newBirdInfoPage, birdNameTransform, false);
            newBirdInfoPage.name = "JournalPageInfo" + ++birdJournalPageNumber;
            newBirdInfoPage.SetActive(false);

            for (int i = 0; i < newBirdInfoPage.transform.childCount; i++)
            {
                newTextComponents = newBirdInfoPage.GetComponentsInChildren<Text>();
            }

            birdNames.AddRange(newTextComponents);
            newBirdInfoPageList.Add(newBirdInfoPage);
        }
    }

}