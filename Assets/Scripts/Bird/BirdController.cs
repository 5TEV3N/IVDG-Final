using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(BirdState))]
public class BirdController : MonoBehaviour
{
    PlayerRaycast playerRaycast;
    BirdState myState;

    public float birdDistance;
    public string birdName;
    public bool playerTookPicture = false;

    private Text discovered;
    private GameObject player;

    void Awake()
    {
        myState = GetComponent<BirdState>();
        player = GameObject.FindGameObjectWithTag("Player");
        discovered = GameObject.Find("BirdNameDisplay").GetComponent<Text>();
        playerRaycast = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRaycast>();
    }

    void Update()
    {
        birdDistance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        birdName = BirdSpawner.currentBirdName;

        if (birdDistance <= 8)
        {
            if (myState.state != BirdState.currentState.interacting)
            {
                myState.state = BirdState.currentState.birdcalls;
                discovered.text = "Discovered a\n " + birdName;
            }
        }

        else if (birdDistance >= 13)
        {
            if (myState.state != BirdState.currentState.interacting)
            {
                myState.state = BirdState.currentState.hidden;
                discovered.text = "";
            }
        }

        if (myState.state == BirdState.currentState.interacting)
        {
            discovered.text = "";
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(0, 0, 0), Time.deltaTime);

            if (playerTookPicture == true)
            {
                myState.state = BirdState.currentState.flyaway;
                playerTookPicture = false;
            }
        }
    }
}
/*
IEnumerable TimeToInteract()
{
    yield return new WaitForSeconds(1f);
    myState.state = BirdState.currentState.flyaway;
}
*/
