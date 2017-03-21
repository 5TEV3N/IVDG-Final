using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BirdState))]
public class BirdController : MonoBehaviour
{
    PlayerRaycast playerRaycast;
    BirdState myState;
    GameUI gameUI;

    public string birdName;
    public float birdDistance;                      // distance between bird and player
    public float birdTriggerBirdcalls;              // distance to trigger the birdcalls state 
    public float birdTriggerHidden;                 // distance to trigger the hidden state
    public bool playerTookPicture = false;          // if the player took a screenshot, bird switches state to runaway. This is subject to change

    private GameObject player;

    void Awake()
    {
        myState = GetComponent<BirdState>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerRaycast = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRaycast>();
        gameUI = GameObject.FindGameObjectWithTag("UI").GetComponent<GameUI>();
    }

    void Update()
    {
        birdDistance = Vector3.Distance(gameObject.transform.position, player.transform.position);

        if (birdDistance <= 8)
        {
            if (myState.state != BirdState.currentState.interacting)
            {
                myState.state = BirdState.currentState.birdcalls;
                gameUI.BirdDiscovered(true);
            }
        }

        else if (birdDistance >= 13)
        {
            if (myState.state != BirdState.currentState.interacting)
            {
                myState.state = BirdState.currentState.hidden;
                gameUI.BirdDiscovered(false);
            }
        }

        if (myState.state == BirdState.currentState.interacting)
        {
            gameUI.BirdDiscovered(false);
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(0, 0, 0), Time.deltaTime);

            if (playerTookPicture == true)
            {
                myState.state = BirdState.currentState.flyaway;
                playerTookPicture = false;
            }
        }
    }
}