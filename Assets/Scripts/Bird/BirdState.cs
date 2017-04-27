using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdState : MonoBehaviour
{
    BirdSpawner birdSpawner;
    BirdAudioControl birdAudioControler;
    //AllSongs allSongs;

    public enum currentState { hidden, birdcalls, interacting, flyaway };
    public currentState state;

    void Awake()
    {
        birdSpawner = GameObject.FindGameObjectWithTag("BirdManager").GetComponent<BirdSpawner>();
        birdAudioControler = GameObject.FindGameObjectWithTag("Bird").GetComponent<BirdAudioControl>();
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

                // Audio or visual indicator here
                break;
            case currentState.birdcalls:
                // State: bird calls. player must persude the bird with bird calls inorder to interact

				if (birdAudioControler.birdSingingOn == false && birdAudioControler.birdSuccess == false && birdAudioControler.birdFailure == false)
                {
                    birdAudioControler.SingLoop();
                }

                break;
            case currentState.interacting:
                // State: interacting. Bird is out of hiding and is in plain view to the player

                birdAudioControler.AudioUIControl("hide");
                break;
            case currentState.flyaway:
                // State: runaway. Bird flies away from the player because of reasons

                birdAudioControler.Initialize();
                birdSpawner.NewBirdLocation();
                birdSpawner.BirdNamer();
                birdSpawner.Deconstructor();
                birdSpawner.BirdConstructor();
                break;
        }
    }
}