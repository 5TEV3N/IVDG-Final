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

    public void BirdConstructor()
    {
        currentBirdName = birdPrefix[Random.Range(0, birdPrefix.Length)] +" "+ birdName[Random.Range(0, birdName.Length)] + " " + birdLastName[Random.Range(0, birdLastName.Length)];

        GameObject head = headPart[Random.Range(0, headPart.Length)];
        GameObject body = bodyPart[Random.Range(0, bodyPart.Length)];
        GameObject leftWing = leftWingPart[Random.Range(0, leftWingPart.Length)];
        GameObject rightWing = rightWingPart[Random.Range(0, rightWingPart.Length)];

        spawnBird.SpawnObjectAtSpotWithParent(headLocation,headLocation, head);
        spawnBird.SpawnObjectAtSpotWithParent(bodyLocation,bodyLocation, body);
        spawnBird.SpawnObjectAtSpotWithParent(leftWingLocation, leftWingLocation, leftWing);
        spawnBird.SpawnObjectAtSpotWithParent(rightWingLocation,rightWingLocation, rightWing);
    }

    void Start()
    {
        BirdConstructor();                                                                                              //For testing. This should be replaced with the states. If the state is runaway, run the constructor again.
        bird.transform.position = birdSpawnLocation[Random.Range(0, birdSpawnLocation.Length)].transform.position;
    }
}
