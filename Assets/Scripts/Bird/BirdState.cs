using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdState : MonoBehaviour
{
    BirdSpawner birdSpawner;

    public enum currentState { hidden, birdcalls, interacting, flyaway };
    public currentState state;
    public AudioSource birdsong;

    private AudioSource audiospec;
    private float originalVol = 0f;

    void Awake()
    {
        birdsong = GameObject.Find("AudioTestBirdsongs").GetComponent<AudioSource>();                                    // Reminder, birdcalls should be attached to the bird! Change later
        audiospec = GameObject.Find("AudioSpectrum").GetComponent<AudioSource>();                                        // Reminder to replace GameObject.Find!
        birdSpawner = GameObject.FindGameObjectWithTag("BirdManager").GetComponent<BirdSpawner>();

        birdsong.volume = originalVol;
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

                birdsong.volume = Mathf.Lerp(birdsong.volume, originalVol, Time.deltaTime);
                audiospec.volume = Mathf.Lerp(birdsong.volume, originalVol, Time.deltaTime);
                break;
            case currentState.birdcalls:
                print("State: bird calls. player must persude the bird with bird calls inorder to interact.");

                birdsong.volume = Mathf.Lerp(birdsong.volume, 0.5f, Time.deltaTime);
                audiospec.volume = Mathf.Lerp(birdsong.volume, 0.5f, Time.deltaTime);
                AudioSpectrum.instance.SetClosestBird(this);
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
