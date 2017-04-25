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
    public Transform[] birdInteractionZones;

    private bool pickedInteractionZone;
    private GameObject player;
    private Animator birdAnimatorComponent;
    private Transform chosenInteractionZone;
    private Transform pickingChosenLocation;
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
        gameObject.transform.forward = player.transform.position;

        if (myState.state == BirdState.currentState.hidden)
        {
            gameUI.BirdDiscovered(false);
        }

        #region Hidden to Birdcall
        // if the player is close to the bird 
        if (birdDistance <= birdTriggerBirdcalls)
        {
            if (myState.state != BirdState.currentState.interacting)
            {
                myState.state = BirdState.currentState.birdcalls;
                pickingChosenLocation = birdInteractionZones[Random.Range(0, birdInteractionZones.Length)];
                float InteractionZoneDistance = Vector3.Distance(player.transform.position, pickingChosenLocation.transform.position);

                if (InteractionZoneDistance < birdDistance)
                {
                    pickedInteractionZone = true;
                    chosenInteractionZone = pickingChosenLocation;
                }

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
            BirdMover(chosenInteractionZone);

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

    public void BirdMover(Transform pos)
    {
        if (pickedInteractionZone == true)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, pos.transform.position, Time.deltaTime * 2);
            CurrentBirdAnimation("caught");
        }
        else { Debug.Log("Error. Something went wrong!"); }
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
