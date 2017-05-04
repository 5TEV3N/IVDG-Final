using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdState : MonoBehaviour
{
    BirdSpawner birdSpawner;
    BirdAudioControl birdAudioControler;
    GameUI gameUI;
    BasicTimer timer = new BasicTimer();

    //AllSongs allSongs;

    public enum currentState { hidden, birdcalls, interacting, flyaway };
    public currentState state;
    public bool tutorialSession;

    void Awake()
    {
        birdSpawner = GameObject.FindGameObjectWithTag("BirdManager").GetComponent<BirdSpawner>();
        birdAudioControler = GameObject.FindGameObjectWithTag("Bird").GetComponent<BirdAudioControl>();
        gameUI = GameObject.FindGameObjectWithTag("UI").GetComponent<GameUI>();
        //allSongs = GameObject.Find("AllSongs").GetComponent<AllSongs>();
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
                int tutorial = 0;
                tutorial++;
                if (tutorial <= 1) { gameUI.InteractionIconsFade(false);}

                break;
            case currentState.birdcalls:
                // State: bird calls. player must persude the bird with bird calls inorder to interact
                gameUI.DisplayBirdcallsIcons(true);
                if (birdAudioControler.birdSingingOn == false && birdAudioControler.birdSuccess == false && birdAudioControler.birdFailure == false){birdAudioControler.SingLoop();}

                break;
            case currentState.interacting:
                // State: interacting. Bird is out of hiding and is in plain view to the player
                gameUI.InteractionIconsFade(true);
                gameUI.DisplayBirdcallsIcons(false);
                birdAudioControler.AudioUIControl("hide");

                break;
            case currentState.flyaway:
                // State: runaway. Bird flies away from the player because of reasons
                birdSpawner.NewBirdLocation();
                birdSpawner.Deconstructor();
                timer.CountDownFrom(2);
                if (timer.timerLeft == 0)
                {
                    birdAudioControler.Initialize();
                    birdSpawner.BirdNamer();
                    birdSpawner.BirdConstructor();
                }

                break;
        }
    }
}