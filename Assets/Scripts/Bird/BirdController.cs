using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BirdState))]
public class BirdController : MonoBehaviour
{
    BirdState myState;
    GameUI gameUI;

    public float birdDistance;                      // distance between bird and player
    public float maxDistance = 500f;                // max distance before the bird changes spawn points;
    public float birdTriggerBirdcalls;              // distance to trigger the birdcalls state 
    public float birdTriggerHidden;                 // distance to trigger the hidden state
    public bool playerTookPicture = false;          // if the player took a screenshot, bird switches state to runaway. This is subject to change

    private GameObject player;

    void Awake()
    {
        myState = GetComponent<BirdState>();
        player = GameObject.FindGameObjectWithTag("Player");
        gameUI = GameObject.FindGameObjectWithTag("UI").GetComponent<GameUI>();
    }

    void Update()
    {
        birdDistance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        
        // if the player is close to the bird 
        if (birdDistance <= birdTriggerBirdcalls)
        {
            if (myState.state != BirdState.currentState.interacting)
            {
                myState.state = BirdState.currentState.birdcalls;
                gameUI.BirdDiscovered(true);
            }
        }
        
        // else, if the the player is far from the bird
        else if (birdDistance >= birdTriggerHidden)
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
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(0, 0, 0), Time.deltaTime);  // changes this later

            if (playerTookPicture == true)
            {
                myState.state = BirdState.currentState.flyaway;
                playerTookPicture = false;
            }
        }

        if (birdDistance >= maxDistance)
        {
            myState.state = BirdState.currentState.flyaway;
        }
    }
}