using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BirdState))]
public class BirdController : MonoBehaviour
{
    //BasicTimer interactionToFlyaway = new BasicTimer();
	//BasicTimer hiddenToFlyAway = new BasicTimer();

    BirdAudioControl birdAudioControler;
    BirdState myState;
    GameUI gameUI;
    
    public enum currentAnimation { caught, flyaway, reset };
    public currentAnimation animationState;

    [Header("Values")]
    public AnimationCurve takeOff;
    public float birdDistance;                                                      // distance between bird and player
    public float birdTriggerBirdcalls = 15f;                                        // distance to trigger the birdcalls state 
    public float birdTriggerFlyaway = 35f;                                          // distance to trigger the hidden state

	[Header ("Timers")]
	public float hiddenToFlyawayTimer = 1f;
	public float interactionToFlyawayTimer = 1f;
	public float hToFOriginal = 100f;
	public float iToFOriginal = 100f;

    [Header ("Logic Check")]
    public bool playerTookPicture = false;                                          // if the player took a screenshot, bird switches state to runaway. This is subject to change

    [Header("Containers")]
    public Transform[] birdInteractionZones;
    public GameObject BirdExitZone;                                                 // container where the bird will fly towards when the bird finishes it's cycle

    private bool pickedInteractionZone;                                             // value to check if the script has chosen a zone for the bird to land
    private string[] aniBoolName = new string[] { "isPecking", "isIdle" };          // an array of different interaction animation states can add more into the array but be sure to edit the code below!
    private string randomIdle;                                                      // the container for the chosen idle out of the array above
    private GameObject player;                                                      // refference to the player
    private Animator birdAnimatorComponent;                                         // self explanatory
    private Transform pickingChosenLocation;                                        // chosing a random transform in birdInteractionZones 
    private Transform chosenInteractionZone;                                        // the chosen zone where the bird is going to land

    void Awake()
    {
        myState = GetComponent<BirdState>();
        birdAnimatorComponent = GetComponent<Animator>();
        birdAudioControler = GetComponent<BirdAudioControl>();
        player = GameObject.FindGameObjectWithTag("Player");
        gameUI = GameObject.FindGameObjectWithTag("UI").GetComponent<GameUI>();
        hiddenToFlyawayTimer = hToFOriginal;
		interactionToFlyawayTimer = iToFOriginal;
	}

    void Update()
    {
        birdDistance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        gameObject.transform.forward = player.transform.position;

        #region Hidden to Birdcall

        if (myState.state == BirdState.currentState.hidden)
        {
            gameUI.BirdDiscovered(false);
        }

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

                randomIdle = aniBoolName[Random.Range(0, aniBoolName.Length)];

                gameUI.BirdDiscovered(true);

            }
        }
        #endregion

        #region Hidden to Flyaway
        // else, if the the player is far from the bird after triggering the birdcall state

		if (myState.state == BirdState.currentState.hidden)
		{

			if (birdDistance > birdTriggerFlyaway)
			{
				if (myState.state != BirdState.currentState.interacting)
				{
					hiddenToFlyawayTimer -= Time.time;
					print ("HIDDEN TO FLYAWAY : " + hiddenToFlyawayTimer);
					if (hiddenToFlyawayTimer <= 0)
					{
						gameUI.BirdDiscovered(false);
						CurrentBirdAnimation(currentAnimation.reset);
						myState.state = BirdState.currentState.flyaway;
						hiddenToFlyawayTimer = hToFOriginal;
					}
				}
			}
		}

        #endregion

        #region Birdcall to Interaction or Flyaway

        if (birdAudioControler.birdFailure == false && birdAudioControler.birdSuccess == true)
        {
            myState.state = BirdState.currentState.interacting;
        }

        if (birdAudioControler.birdFailure == true && birdAudioControler.birdSuccess == false)
        {
            myState.state = BirdState.currentState.flyaway;
        }

        if (myState.state == BirdState.currentState.interacting)
        {
            gameUI.BirdDiscovered(false);
            BirdMover(chosenInteractionZone);

            if (playerTookPicture == true)
            {
				interactionToFlyawayTimer -= Time.time;
				print ("INTERACTION TO FLYAWAY " + interactionToFlyawayTimer);
				if (interactionToFlyawayTimer <= 0)
				{
					CurrentBirdAnimation(currentAnimation.reset);
					myState.state = BirdState.currentState.flyaway;
					playerTookPicture = false;
					interactionToFlyawayTimer = iToFOriginal;
					print ("BYE BYE");
				}
				/*
                CurrentBirdAnimation(currentAnimation.flyaway);
				interactionToFlyaway.ResetTimer();
				print ("INTERACTION TO FLYAWAY " + interactionToFlyaway.timerLeft);
				interactionToFlyaway.CountDownFrom(1);
				if (interactionToFlyaway.timerLeft == 0)
                {
                    CurrentBirdAnimation(currentAnimation.reset);
                    myState.state = BirdState.currentState.flyaway;
                    playerTookPicture = false;
					print ("END " +interactionToFlyaway);
                }
                */
            }
        }
        #endregion

    }

    public void BirdMover(Transform pos)
    {
        if (pickedInteractionZone == true)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, pos.transform.position, Time.deltaTime * 2);
            CurrentBirdAnimation(currentAnimation.caught);
        }
        else { Debug.Log("Error. Did not see player and went straight to the next state. Check Hidden to Birdcall In BirdController"); }
    }

    public void CurrentBirdAnimation(currentAnimation animation)
    {
        if (animation == currentAnimation.caught)
        {
            birdAnimatorComponent.SetBool("isCaught", true);
            birdAnimatorComponent.SetBool("isIdle", true);
            birdAnimatorComponent.SetBool("isFlyingAway", false);
            birdAnimatorComponent.SetBool("isHidden", false);
            if (randomIdle == "isPecking") { birdAnimatorComponent.SetBool("isIdle", false); birdAnimatorComponent.SetBool("isPecking", true); }
            if (randomIdle == "isIdle") { birdAnimatorComponent.SetBool("isIdle", true); birdAnimatorComponent.SetBool("isPecking", false); }

        }

        if (animation == currentAnimation.flyaway)
        {
            birdAnimatorComponent.SetBool("isCaught", false);
            birdAnimatorComponent.SetBool("isIdle", false);
            birdAnimatorComponent.SetBool("isPecking", false);
            birdAnimatorComponent.SetBool("isFlyingAway", true);
            birdAnimatorComponent.SetBool("isHidden", true);
        }

        if (animation == currentAnimation.reset)
        {
            birdAnimatorComponent.SetBool("isCaught", false);
            birdAnimatorComponent.SetBool("isIdle", false);
            birdAnimatorComponent.SetBool("isPecking", false);
            birdAnimatorComponent.SetBool("isFlyingAway", false);
            birdAnimatorComponent.SetBool("isHidden", true);
        }
    }
}
