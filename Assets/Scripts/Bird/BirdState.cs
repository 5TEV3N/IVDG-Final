using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdState : MonoBehaviour
{
    BirdSpawner birdSpawner;
    BirdController birdController;
    BirdAudioControl birdAudioControler;
    GameUI gameUI;
    InputManager3D inputManager;

    public enum currentState { hidden, birdcalls, interacting, flyaway };
    public currentState state;

    [Header ("Logic Checks")]
	public bool tutorialSession;
	public int tutorialCheck;
    bool lastCheck;

	void Awake()
    {
        birdSpawner = GameObject.FindGameObjectWithTag("BirdManager").GetComponent<BirdSpawner>();
        birdAudioControler = GetComponent<BirdAudioControl>();
        birdController = GetComponent<BirdController>();
        gameUI = GameObject.FindGameObjectWithTag("UI").GetComponent<GameUI>();
        inputManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InputManager3D>();
    }

    void Update()
    {
        updateState(state);

    }

    public void updateState(currentState birdstate)                                                                      
    {
        switch (birdstate)
        {
            case currentState.hidden:
                // State: hidden. Bird is hidden and singing

                gameUI.DisplayBirdcallsIcons(false);
                birdController.playerTookPicture = false;

                if (tutorialCheck <= 1) 
				{ 
					gameUI.InteractionIconsFade("Start"); 
					tutorialSession = true; 
					gameUI.TutorialTexts(false, false, false);
				}
				
				if (tutorialCheck > 1)
				{
					tutorialSession = false;
                    lastCheck = false;
                    gameUI.TutorialTexts(false, false, false);
                }

                break;
			case currentState.birdcalls:
                // State: bird calls. player must persude the bird with bird calls inorder to interact	
                gameUI.DisplayBirdcallsIcons (true);

                if (tutorialCheck > 1)
                {
                    tutorialSession = false;
                    lastCheck = false;
                    gameUI.TutorialTexts(false, false, false);
                }

                if (tutorialSession == true) 
				{
					gameUI.TutorialTexts (true,false,false);
				}

                // keeps doing the singloop until it reaches to a conclusion. once that's done, change states
                if (birdAudioControler.birdSingingOn == false && birdAudioControler.birdSuccess == false && birdAudioControler.birdFailure == false)
				{ 
					birdAudioControler.SingLoop(); 
				}
				
                break;
            case currentState.interacting:
                // State: interacting. Bird is out of hiding and is in plain view to the player
                if (tutorialSession == true)
                {
                    lastCheck = true;
                    gameUI.TutorialTexts(false, true, false);
                    if (inputManager.cameraButtonCheck == 1)
                    {
                        gameUI.TutorialTexts(false, false, false);
                    }
                }

                gameUI.InteractionIconsFade("Success");
                gameUI.DisplayBirdcallsIcons(false);
                birdAudioControler.AudioUIControl("hide");

                tutorialCheck = 2;

                break;
		case currentState.flyaway:
                // State: runaway. Bird flies away from the player because of reasons
                if (lastCheck == true)
                {
                    gameUI.TutorialTexts(false, false, true);
                }
                gameUI.InteractionIconsFade("NullCamera");
                birdSpawner.BirdLocationBuffer ();
                StartCoroutine("BufferTime");
                break;
        }
    }
    public IEnumerator BufferTime()
    {
        yield return new WaitForSeconds(5);
        birdSpawner.NewBirdLocation();
        birdSpawner.Deconstructor();
        birdAudioControler.Initialize();
        birdSpawner.BirdNamer();
        birdSpawner.BirdConstructor();
        StopCoroutine("BufferTime");
    }
}
