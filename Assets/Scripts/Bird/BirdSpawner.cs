using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    // Spawn a bird. State is hidden. Put them in a random location. Give them their name.
    BirdState myState;
    BirdController birdController;
    BirdAudioControl birdAudioControl;

    public static string currentBirdName;
    public GameObject bird;
    public Transform[] birdSpawnLocation;

    [Header("The Trailer Button")]
    public bool imDoingTheTrailerRightNow;

    [Header("Bird's Container")]
    public Transform headLocation;
    public Transform bodyLocation;
    public Transform leftWingLocation;
    public Transform rightWingLocation;

    [Header("Bird Name")]
    public string[] birdPrefix;
    public string[] birdName;
    public string[] birdLastName;

    [Header("Bird Parts")]
    public GameObject[] headPart;
    public GameObject[] bodyPart;
    public GameObject[] leftWingPart;
    public GameObject[] rightWingPart;

    private GameObject currentHeadInstance;
    private GameObject currentBodyInstance;
    private GameObject currentLeftWingInstance;
    private GameObject currentRightWingInstance;

    void Awake()
    {
        myState = GameObject.FindGameObjectWithTag("Bird").GetComponent<BirdState>();
        birdController = GameObject.FindGameObjectWithTag("Bird").GetComponent<BirdController>();
        birdAudioControl = GameObject.FindGameObjectWithTag("Bird").GetComponent<BirdAudioControl>();
    }

    public void BirdNamer()
    {
        currentBirdName = birdPrefix[Random.Range(0, birdPrefix.Length)] + " " + birdName[Random.Range(0, birdName.Length)] + " " + birdLastName[Random.Range(0, birdLastName.Length)];
    }

    public void BirdConstructor()
    {
        // Depricated, please incorprate ana's model instead
        //GameObject head = headPart[Random.Range(0, headPart.Length)];
        //GameObject body = bodyPart[Random.Range(0, bodyPart.Length)];
        //GameObject leftWing = leftWingPart[Random.Range(0, leftWingPart.Length)];
        //GameObject rightWing = rightWingPart[Random.Range(0, rightWingPart.Length)];
        //
        //currentHeadInstance = Instantiate(head, headLocation.transform, false);
        //currentBodyInstance = Instantiate(body, bodyLocation.transform, false);
        //currentLeftWingInstance = Instantiate(leftWing, leftWingLocation.transform, false);
        //currentRightWingInstance = Instantiate(rightWing, rightWingLocation.transform, false);
        //birdAudioControl.Initialize();
        myState.state = BirdState.currentState.hidden;
        
    }

    public void Deconstructor()
    {
        // Depricated, please incorprate ana's model instead
        //DestroyObject(currentHeadInstance);
        //DestroyObject(currentBodyInstance);
        //DestroyObject(currentLeftWingInstance);
        //DestroyObject(currentRightWingInstance);
    }

    public void NewBirdLocation()
    {
        // please spawn it at a close range using birdController
        bird.transform.position = birdSpawnLocation[Random.Range(0, birdSpawnLocation.Length)].transform.position;
    }

    void Start()
    {
        if (imDoingTheTrailerRightNow == false)
        {
            BirdNamer();
            BirdConstructor();
            NewBirdLocation();
        }
        else { BirdNamer(); }
    }
}