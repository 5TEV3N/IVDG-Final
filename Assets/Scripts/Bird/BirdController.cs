using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BirdState))]
public class BirdController : MonoBehaviour
{
    BirdAudioControl birdAudioControler;
    BirdState myState;
    GameUI gameUI;
    
    public enum currentAnimation { caught, flyaway, reset };                        // enum of the different animation states of the bird
    public currentAnimation animationState;

    [Header("Values")]
    public float birdDistance;                                                      // distance between bird and player
    public float birdTriggerBirdcalls = 15f;                                        // distance to trigger the birdcalls state 
    public float birdTriggerFlyaway = 35f;                                          // distance to trigger the hidden state

    [Header ("Logic Check")]
    public bool playerTookPicture = false;                                          // if the player took a screenshot, bird switches state to runaway. This is subject to change

    [Header("Containers")]
    public Transform[] birdInteractionZones;                                        // contains all of the bird's landingzones
    public GameObject wingFlapsSFX;                                                 // contains the gameobject that has the audiosource for the wingflapssfx

    private bool pickedInteractionZone;                                             // value to check if the script has chosen a zone for the bird to land
    private string[] aniBoolName = new string[] { "isPecking", "isIdle" };          // an array of different interaction animation states can add more into the array but be sure to edit the code below!
    private string randomIdle;                                                      // the container for the chosen idle out of the array above
    private GameObject player;                                                      // refference to the player
    private Animator birdAnimatorComponent;                                         // self explanatory
    private Transform pickingChosenLocation;                                        // chosing a random transform in birdInteractionZones 
    private Transform chosenInteractionZone;                                        // the chosen zone where the bird is going to land
    private bool flyingSFXStarted;                                                  // logic to check for the bird flapping

    void Awake()
    {
        // Refferences
        myState = GetComponent<BirdState>();
        birdAnimatorComponent = GetComponent<Animator>();
        birdAudioControler = GetComponent<BirdAudioControl>();
        player = GameObject.FindGameObjectWithTag("Player");
        gameUI = GameObject.FindGameObjectWithTag("UI").GetComponent<GameUI>();
    }

    void Update()
    {
        birdDistance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        gameObject.transform.forward = player.transform.position; // IMPORTANT, TO BE REPLACED SOMETHING MORE BELIEVEABLE FOR THE VERNISAGE!

        // this section revolves around the bird interacting with the states and changing them accordingly

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
                // picking out where the bird will land
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
                    StartCoroutine("GettingReadyToLeave");
                }
			}
		}

        #endregion

        #region Birdcall to Interaction or Flyaway
        //failure/success logic from the birdcalls
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
            PlayWingFlaps(true);
            if (playerTookPicture == true)
            {
                CurrentBirdAnimation(currentAnimation.flyaway);
                PlayWingFlaps(false);
                myState.state = BirdState.currentState.flyaway;
            }
        }

        #endregion

    }

    public void PlayWingFlaps(bool isFlying)
    {
        if (isFlying == true)
        {
            if (flyingSFXStarted == false)
            {
                wingFlapsSFX.GetComponent<Wingflaps>().FlapPlay();
                flyingSFXStarted = true;
            }
        }
        if (isFlying == false)
        {
            flyingSFXStarted = false;
        }
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

    public IEnumerator GettingReadyToLeave()
    {
        yield return new WaitForSeconds(5f);
        myState.state = BirdState.currentState.flyaway;
        StopCoroutine("GettingReadyToLeave");
    }
}
