using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdState : MonoBehaviour
{
    BirdSpawner birdSpawner;
    BirdAudioControl birdAudioControler;
    AllSongs allSongs;

    public enum currentState { hidden, birdcalls, interacting, flyaway };
    public currentState state;

    void Awake()
    {
        birdSpawner = GameObject.FindGameObjectWithTag("BirdManager").GetComponent<BirdSpawner>();
        birdAudioControler = GameObject.FindGameObjectWithTag("Bird").GetComponent<BirdAudioControl>();
        allSongs = GameObject.Find("AllSongs").GetComponent<AllSongs>();
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
                print("State: hidden. Bird is hidden and singing");

                // if the distance is too great between the bird and player, then call NewBirdLocation
                break;
            case currentState.birdcalls:
                print("State: bird calls. player must persude the bird with bird calls inorder to interact.");

                //if (birdAudioControler.isWhistleGood?) { state = currentState.Interacting }
                break;
            case currentState.interacting:
                print("State: interacting. Bird is out of hiding and his in plain view to the player");

                break;
            case currentState.flyaway:
                print("State: runaway. Bird flies away from the player because of reasons");

                birdSpawner.NewBirdLocation();
                birdSpawner.Deconstructor();
                birdSpawner.BirdNamer();
                birdSpawner.BirdConstructor();
                break;
        }
    }
}
/*
 * Kaermack's suggestion:
 * Birdcall must be randomized and assigned to the bird.
 * For the AudioSpectrum, look for the bird that is closest to the player.
 * Then, that bird must push the birdcall into the AudioSpectrum
 * Set the audio input to the birdcall.
*/
