using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    //Spawn a bird. State is hidden. Put them in a random location. Give them their name
    Spawner spawnBird = new Spawner();

    public GameObject bird;
    public Transform[] birdSpawnLocation;
    public static string currentBirdName;

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

    private GameObject currentHead;
    private GameObject currentBody;
    private GameObject currentLeftWing;
    private GameObject currentRightWing;

    public bool newBird;
    public void BirdConstructor()
    {
        if (newBird)
        {
            currentBirdName = birdPrefix[Random.Range(0, birdPrefix.Length)] + " " + birdName[Random.Range(0, birdName.Length)] + " " + birdLastName[Random.Range(0, birdLastName.Length)];

            currentHead = headPart[Random.Range(0, headPart.Length)];
            currentBody = bodyPart[Random.Range(0, bodyPart.Length)];
            currentLeftWing = leftWingPart[Random.Range(0, leftWingPart.Length)];
            currentRightWing = rightWingPart[Random.Range(0, rightWingPart.Length)];

            spawnBird.SpawnObjectAtSpotWithParent(headLocation, headLocation, currentHead);
            spawnBird.SpawnObjectAtSpotWithParent(bodyLocation, bodyLocation, currentBody);
            spawnBird.SpawnObjectAtSpotWithParent(leftWingLocation, leftWingLocation, currentLeftWing);
            spawnBird.SpawnObjectAtSpotWithParent(rightWingLocation, rightWingLocation, currentRightWing);
        }
        else 
        {
            DestroyImmediate(currentHead.gameObject);
            DestroyImmediate(currentBody.gameObject);
            DestroyImmediate(currentLeftWing.gameObject);
            DestroyImmediate(currentRightWing.gameObject);
            newBird = true;
            //find a way to remove the current gameobjects
        }
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
