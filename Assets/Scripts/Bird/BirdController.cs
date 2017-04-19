using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BirdState))]
public class BirdController : MonoBehaviour
{
    BasicTimer timer = new BasicTimer();
    BirdState myState;
    GameUI gameUI;

    public float birdDistance;                      // distance between bird and player
    public float birdTriggerBirdcalls = 15f;        // distance to trigger the birdcalls state 
    public float birdTriggerFlyaway = 35f;          // distance to trigger the hidden state
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
        
        #region Hidden to Birdcall
        // if the player is close to the bird 
        if (birdDistance <= birdTriggerBirdcalls)
        {
            if (myState.state != BirdState.currentState.interacting)
            {
                myState.state = BirdState.currentState.birdcalls;
                gameUI.BirdDiscovered(true);
            }
        }
        #endregion

        #region Hidden to Flyaway
        // else, if the the player is far from the bird after triggering the birdcall state
        else if (birdDistance > birdTriggerFlyaway)
        {
            if (myState.state != BirdState.currentState.interacting)
            {
                timer.CountDownFrom(3);
                if (timer.timerLeft == 0)
                {
                    print("Player couldn't find the bird in time, Reseting bird and location...");
                    myState.state = BirdState.currentState.flyaway;
                    gameUI.BirdDiscovered(false);
                    timer.ResetTimer();
                }
            }
        }
        #endregion

        #region Hidden to Interaction
        if (myState.state == BirdState.currentState.interacting)
        {
            gameUI.BirdDiscovered(false);
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(0, 0, 0), Time.deltaTime);  // changes this later

            if (playerTookPicture == true)
            {
                timer.CountDownFrom(5);
                if (timer.timerLeft == 0)
                {
                    // Change animation to flying animation. At end of animation , change state.
                    myState.state = BirdState.currentState.flyaway;
                    playerTookPicture = false;
                }
            }
        }
        #endregion

    }
}