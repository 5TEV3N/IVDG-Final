﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    //Spawn a bird. State is hidden. Put them in a random location. Give them their name
    //Spawner spawnBird = new Spawner();
    BirdState myState;

    public static string currentBirdName;
    public GameObject bird;
    public Transform[] birdSpawnLocation;

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
    }

    public void BirdConstructor()
    {
        currentBirdName = birdPrefix[Random.Range(0, birdPrefix.Length)] + " " + birdName[Random.Range(0, birdName.Length)] + " " + birdLastName[Random.Range(0, birdLastName.Length)];

        GameObject head = headPart[Random.Range(0, headPart.Length)];
        GameObject body = bodyPart[Random.Range(0, bodyPart.Length)];
        GameObject leftWing = leftWingPart[Random.Range(0, leftWingPart.Length)];
        GameObject rightWing = rightWingPart[Random.Range(0, rightWingPart.Length)];
        
        currentHeadInstance = Instantiate(head, headLocation.transform, false);
        currentBodyInstance = Instantiate(body, bodyLocation.transform, false);
        currentLeftWingInstance = Instantiate(leftWing, leftWingLocation.transform, false);
        currentRightWingInstance = Instantiate(rightWing, rightWingLocation.transform, false);
                                                          
        myState.state = BirdState.currentState.hidden;
    }

    public void Deconstructor()
    {
        DestroyObject(currentHeadInstance);
        DestroyObject(currentBodyInstance);
        DestroyObject(currentLeftWingInstance);
        DestroyObject(currentRightWingInstance);
    }

    public void NewBirdLocation()
    {
        bird.transform.position = birdSpawnLocation[Random.Range(0, birdSpawnLocation.Length)].transform.position;
    }

    void Start()
    {
        BirdConstructor();
        NewBirdLocation();
    }
}