using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    BirdState myState;
    BirdAudioControl birdAudioControl;

    public static string currentBirdName;                   // name of current bird
    public GameObject bird;                                 // contains the bird's gameobject
    public Transform[] birdSpawnLocation;                   // contains the possible areas the bird can spawn in
	public GameObject bufferLocation;                       // contains the location where the bird will stay during it's buffer period
                                                            
    [Header("The Trailer Button")]                          
    public bool imDoingTheTrailerRightNow;                  // logic to check whether or not you're doing the trailer
                                                            
    [Header("Bird Name")]                                   
    public string[] birdPrefix;                             //  describer/rarity
    public string[] birdName;                               //  main name
    public string[] birdLastName;                           //  last name
                                                            
    [Header("Bird Parts")]                                  
    public GameObject[] headPieceParts;                     // the many bird head piece that the bird can spawn with
    public GameObject[] baseBodyParts;                      // the many body types the bird can spawn with
                                                            
    public GameObject[] topBeakParts;                       // the many top beaks that the bird can spawn with
    public GameObject[] bottomBeakParts;                    // the many bottom beaks that the bird can spawn with
                                                            
    public GameObject[] tailParts;                          // the many tail pieces the bird can spawn with
    public GameObject[] wingsParts;                         // the many wings pieces the bird can spawn with
                                                            
    private GameObject currentHeadPiecePart;                // the current head piece the bird spawned with
    private GameObject currentBaseBodyPart;                 // the current body piece the bird spawned with
                                                            
    private GameObject currentTopBeakParts;                 // the current top beak the bird spawned with
    private GameObject currentBottomBeakParts;              // the current bottom beak the bird spawned with
                                                            
    private GameObject currentTailParts;                    // the current tail piece the bird spawned with
    private GameObject currentWingsPart;                    // the current wings piece the bird spawned with


    void Awake()
    {
        myState = GameObject.FindGameObjectWithTag("Bird").GetComponent<BirdState>();
        birdAudioControl = GameObject.FindGameObjectWithTag("Bird").GetComponent<BirdAudioControl>();
    }

    public void BirdNamer()
    {
        // pick a name from each array randomly
        currentBirdName = birdPrefix[Random.Range(0, birdPrefix.Length)] + " " + birdName[Random.Range(0, birdName.Length)] + " " + birdLastName[Random.Range(0, birdLastName.Length)];
    }

    public void BirdConstructor()
    {
        // pick a piece at random and set it to active to true
        currentHeadPiecePart = headPieceParts[Random.Range(0, headPieceParts.Length)];
        currentBaseBodyPart = baseBodyParts[Random.Range(0, baseBodyParts.Length)];

        currentTopBeakParts = topBeakParts[Random.Range(0, topBeakParts.Length)];
        currentBottomBeakParts = bottomBeakParts[Random.Range(0, bottomBeakParts.Length)];

        currentTailParts = tailParts[Random.Range(0, tailParts.Length)];
        currentWingsPart = wingsParts[Random.Range(0, wingsParts.Length)];

        currentHeadPiecePart.SetActive(true);
        currentBaseBodyPart.SetActive(true);

        currentTopBeakParts.SetActive(true);
        currentBottomBeakParts.SetActive(true);
        currentWingsPart.SetActive(true);

        currentTailParts.SetActive(true);
        birdAudioControl.Initialize();
        myState.state = BirdState.currentState.hidden;
        
    }

    public void Deconstructor()
    {
        // set everything to false in preperation for the constructor

        currentHeadPiecePart.SetActive(false);
        currentBaseBodyPart.SetActive(false);

        currentTopBeakParts.SetActive(false);
        currentBottomBeakParts.SetActive(false);

        currentTailParts.SetActive(false);
        currentWingsPart.SetActive(false);
    }

    public void NewBirdLocation()
    {
        // changes the location of the bird. spawns in location 1 for the first time as a tutorial

        if (myState.tutorialSession == true)
        {
            bird.transform.position = birdSpawnLocation[1].transform.position;
        }
        else
        {
            bird.transform.position = birdSpawnLocation[Random.Range(0, birdSpawnLocation.Length)].transform.position;
        }
    }

	public void BirdLocationBuffer()
	{
        // the bird stays here for a couple seconds to avoid the player rencountering it too quickly
		bird.transform.position = bufferLocation.transform.position;
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

    void Update()//DEBUG
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Deconstructor();
        }
    }
}