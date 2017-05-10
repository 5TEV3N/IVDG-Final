using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdState : MonoBehaviour
{
    BirdSpawner birdSpawner;
    BirdController birdController;
    BirdAudioControl birdAudioControler;
    GameUI gameUI;


    public enum currentState { hidden, birdcalls, interacting, flyaway };
    public currentState state;

    [Header ("Logic Checks")]
	public bool tutorialSession;
	public int tutorialCheck;

	void Awake()
    {
        birdSpawner = GameObject.FindGameObjectWithTag("BirdManager").GetComponent<BirdSpawner>();
        birdAudioControler = GetComponent<BirdAudioControl>();
        birdController = GetComponent<BirdController>();
        gameUI = GameObject.FindGameObjectWithTag("UI").GetComponent<GameUI>();
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
					gameUI.InteractionIconsFade(false); 
					tutorialSession = true; 
				}
				
				if (tutorialCheck > 1)
				{
					tutorialSession = false;
				}

                break;
			case currentState.birdcalls:
                // State: bird calls. player must persude the bird with bird calls inorder to interact	
                gameUI.DisplayBirdcallsIcons (true);
				
				if (tutorialSession == true) 
				{
					gameUI.TutorialTexts (true,false,false);
				}

				if (birdAudioControler.birdSingingOn == false && birdAudioControler.birdSuccess == false && birdAudioControler.birdFailure == false)
				{ 
					birdAudioControler.SingLoop(); 
				}
				
				//tutorialCheck++;
                break;
            case currentState.interacting:
                // State: interacting. Bird is out of hiding and is in plain view to the player
                gameUI.InteractionIconsFade(true);
                gameUI.DisplayBirdcallsIcons(false);
                birdAudioControler.AudioUIControl("hide");
				tutorialCheck++;

                break;
		case currentState.flyaway:
                // State: runaway. Bird flies away from the player because of reasons
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
