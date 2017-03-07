using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdInfoToJournal : MonoBehaviour
{
    // NOT THE BEST WAY TO GO ABOUT THIS. PLEASE FIGURE OUT A MORE OPTIMAL WAY OF GOING ABOUT THIS LATER
    BirdController currentBirdName;
    [Space(10)]
    public GameObject newBirdInfoPage;
    public GameObject newBirdInputField; 
    public Transform birdNameTransform;

    [Header("Lists")]
    public List<Text> birdNames = new List<Text>();
    public List<InputField> inputFieldsList = new List<InputField>();
    public List<GameObject> newBirdInfoPageList = new List<GameObject>();
    public List<GameObject> newInputFieldsList = new List<GameObject>();

    private Text[] newTextComponents;
    private InputField[] newInputFieldComponents;
    private int birdJournalPageNumber;
    private int inputFieldNumber;
    private int birdInfoSlot;

    public void AddInfo()
    {
        currentBirdName = GameObject.Find("Bird").GetComponent<BirdController>();
        if (birdInfoSlot < birdNames.Count)
        {
            birdNames[birdInfoSlot].text = currentBirdName.birdName;
            birdInfoSlot++;
        }
        else
        {
            newBirdInputField = Instantiate(newBirdInputField, birdNameTransform, false);
            newBirdInputField.name = "InputFieldsPage" + ++inputFieldNumber;
            newBirdInputField.SetActive(false);

            for (int i = 0; i < newBirdInputField.transform.childCount; i++)
            {
                newInputFieldComponents = newBirdInputField.GetComponentsInChildren<InputField>();
            }

            inputFieldsList.AddRange(newInputFieldComponents);
            newInputFieldsList.Add(newBirdInputField);

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