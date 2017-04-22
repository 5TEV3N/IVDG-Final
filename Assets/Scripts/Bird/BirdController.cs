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
    private Animator birdAnimatorComponent;

    void Awake()
    {
        myState = GetComponent<BirdState>();
        birdAnimatorComponent = GetComponent<Animator>();
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
                    Debug.Log("Player couldn't find the bird in time, Reseting bird and location...");
                    gameUI.BirdDiscovered(false);
                    CurrentBirdAnimation("reset");
                    myState.state = BirdState.currentState.flyaway;
                    timer.ResetTimer();
                }
            }
        }
        #endregion

        #region Hidden to Interaction
        if (myState.state == BirdState.currentState.interacting)
        {
            gameUI.BirdDiscovered(false);
            CurrentBirdAnimation("caught");

            if (playerTookPicture == true)
            {
                CurrentBirdAnimation("flyaway");
                timer.CountDownFrom(5);
                if (timer.timerLeft == 0)
                {
                    myState.state = BirdState.currentState.flyaway;
                    playerTookPicture = false;
                }
            }
        }
        #endregion

    }

    public void CurrentBirdAnimation(string animation)
    {
        if (animation == "caught")
        {
            // move the bird next to the player after reaching this state
            birdAnimatorComponent.SetBool("isCaught", true);
            birdAnimatorComponent.SetBool("isInteracting", true);
            birdAnimatorComponent.SetBool("isPecking", true);
            birdAnimatorComponent.SetBool("isFlyingAway", false);
            birdAnimatorComponent.SetBool("isHidden", false);
            //randomly switch between idle and pecking here
            /*
            string[] ani = new string[] { "Idle", "Pecking" };
            string randomIdle = ani [Random.Range(0,ani.Length)];

            birdAnimatorComponent.Play(randomIdle);
            */
        }

        if (animation == "flyaway")
        {
            birdAnimatorComponent.SetBool("isCaught", false);
            birdAnimatorComponent.SetBool("isInteracting", false);
            birdAnimatorComponent.SetBool("isPecking", false);
            birdAnimatorComponent.SetBool("isFlyingAway", true);
            birdAnimatorComponent.SetBool("isHidden", true);
        }

        if (animation == "reset")
        {
            birdAnimatorComponent.SetBool("isCaught", false);
            birdAnimatorComponent.SetBool("isInteracting", false);
            birdAnimatorComponent.SetBool("isPecking", false);
            birdAnimatorComponent.SetBool("isFlyingAway", false);
            birdAnimatorComponent.SetBool("isHidden", true);
        }
    }
}
