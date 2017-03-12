using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(BirdState))]
public class BirdController : MonoBehaviour
{
    PlayerRaycast playerRaycast;
    BirdState myState;

    public string birdName;
    public float birdDistance;                      // distance between bird and player
    public float birdTriggerBirdcalls;              // distance to trigger the birdcalls state 
    public float birdTriggerHidden;                 // distance to trigger the hidden state
    public bool playerTookPicture = false;          // if the player took a screenshot, bird switches state to runaway. This is subject to change

    private Text discovered;                        // the text that shows when you discovered this bird
    private GameObject player;

    void Awake()
    {
        myState = GetComponent<BirdState>();
        player = GameObject.FindGameObjectWithTag("Player");
        discovered = GameObject.FindGameObjectWithTag("UIBirdName").GetComponent<Text>();
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