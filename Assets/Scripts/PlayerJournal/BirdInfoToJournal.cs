using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdInfoToJournal : MonoBehaviour
{
    // NOT THE BEST WAY TO GO ABOUT THIS. PLEASE FIGURE OUT A MORE OPTIMAL WAY OF GOING ABOUT THIS LATER
    GameUI gameUI;

    [Header("Containers")]
    public GameObject newBirdName;
    public GameObject newBirdInputField; 
    public Transform birdNameTransform;
	public Transform inputFieldTransform;

    [Header ("Values")]
    public int birdInfoSlot;
    public int inputFieldSlot;
    private int birdJournalPageNumber;
	private int inputFieldNumber;
    
	[Header("Lists")]
    public List<Text> birdNames = new List<Text>();
    public List<InputField> inputFieldsList = new List<InputField>();
    public List<GameObject> newBirdNamePageList = new List<GameObject>();
    public List<GameObject> newInputFieldsList = new List<GameObject>();

    private Text[] newTextComponents;
    private InputField[] newInputFieldComponents;

    public void AddInfo()
    {
        gameUI = GameObject.FindGameObjectWithTag("UI").GetComponent<GameUI>();
        if (birdInfoSlot < birdNames.Count)
        {
            birdNames[birdInfoSlot].text = gameUI.birdName;
            birdInfoSlot++;
        }
        else
        {
            newBirdName = Instantiate(newBirdName, birdNameTransform, false);
            newBirdName.name = "BirdName" + ++birdJournalPageNumber;
            newBirdName.SetActive(false);

            newTextComponents = newBirdName.GetComponentsInChildren<Text>();
            birdNames.AddRange(newTextComponents);
            newBirdNamePageList.Add(newBirdName);

            birdNames[birdInfoSlot].text = gameUI.birdName;
            birdInfoSlot++;
        }
    }

    public void AddInputField()
    {
        newBirdInputField = Instantiate(newBirdInputField, inputFieldTransform, false);
        newBirdInputField.name = "InputFieldsPage" + ++inputFieldNumber;
        newBirdInputField.SetActive(false);
        inputFieldSlot++;
        for (int i = 0; i < newBirdInputField.transform.childCount; i++)
        {
            newInputFieldComponents = newBirdInputField.GetComponentsInChildren<InputField>();
        }

        inputFieldsList.AddRange(newInputFieldComponents);
        newInputFieldsList.Add(newBirdInputField);
	}
}
/*
newBirdInputField = Instantiate(newBirdInputField, birdNameTransform, false);
newBirdInputField.name = "InputFieldsPage" + ++inputFieldNumber;
newBirdInputField.SetActive(false);

for (int i = 0; i < newBirdInputField.transform.childCount; i++)
{
    newInputFieldComponents = newBirdInputField.GetComponentsInChildren<InputField>();
}

inputFieldsList.AddRange(newInputFieldComponents);
newInputFieldsList.Add(newBirdInputField);








            if (birdInfoSlot < birdNames.Count)
        {
            //print("if");

            newBirdName = Instantiate(newBirdName, birdNameTransform, false);
            newBirdName.name = "BirdName" + ++birdJournalPageNumber;
            newBirdName.SetActive(true);

            newTextComponents = newBirdName.GetComponentsInChildren<Text>();
            birdNames.AddRange(newTextComponents);
            newBirdNamePageList.Add(newBirdName);

            birdNames[birdInfoSlot].text = currentBirdName.birdName;
            birdInfoSlot++;
        }
        else
        {
*/