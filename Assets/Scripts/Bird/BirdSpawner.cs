using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    BirdState myState;
    //BirdController birdController;
    //BirdAudioControl birdAudioControl;

    public static string currentBirdName;
    public GameObject bird;
    public Transform[] birdSpawnLocation;

    [Header("The Trailer Button")]
    public bool imDoingTheTrailerRightNow;

    [Header("Bird Name")]
    public string[] birdPrefix;
    public string[] birdName;
    public string[] birdLastName;

    [Header("Bird Parts")]
    public GameObject[] headPieceParts;
    public GameObject[] baseBodyParts;
    public GameObject[] beakParts;
    public GameObject[] wingParts;

    private GameObject currentHeadPiecePart;
    private GameObject currentBaseBodyPart;
    private GameObject currentBeakPart;
    private GameObject currentWingPart;

    //COMMENTED OUT THE WING PARTS DUE TO THERE NOT BEING ANY WING PARTS YET, ADD BACK LATER??

    void Awake()
    {
        myState = GameObject.FindGameObjectWithTag("Bird").GetComponent<BirdState>();
        //birdController = GameObject.FindGameObjectWithTag("Bird").GetComponent<BirdController>();
        //birdAudioControl = GameObject.FindGameObjectWithTag("Bird").GetComponent<BirdAudioControl>();
    }

    public void BirdNamer()
    {
        currentBirdName = birdPrefix[Random.Range(0, birdPrefix.Length)] + " " + birdName[Random.Range(0, birdName.Length)] + " " + birdLastName[Random.Range(0, birdLastName.Length)];
    }

    public void BirdConstructor()
    {
        currentHeadPiecePart = headPieceParts[Random.Range(0, headPieceParts.Length)];
        currentBaseBodyPart = baseBodyParts[Random.Range(0, baseBodyParts.Length)];
        currentBeakPart = beakParts[Random.Range(0, beakParts.Length)];
        //currentWingPart = wingParts[Random.Range(0, wingParts.Length)];

        currentHeadPiecePart.SetActive(true);
        currentBaseBodyPart.SetActive(true);
        currentBeakPart.SetActive(true);
        //currentWingPart.SetActive(true);

        myState.state = BirdState.currentState.hidden;
    }

    public void Deconstructor()
    {
        currentHeadPiecePart.SetActive(false);
        currentBaseBodyPart.SetActive(false);
        currentBeakPart.SetActive(false);
        //currentWingPart.SetActive(false);
        BirdConstructor();
    }

    public void NewBirdLocation()
    {
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

    void Update()//DEBUG
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Deconstructor();
        }
    }
}