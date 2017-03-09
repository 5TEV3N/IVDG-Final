using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdState : MonoBehaviour
{
    BirdSpawner birdSpawner;

    public enum currentState { hidden, birdcalls, interacting, flyaway };
    public currentState state;
    public AudioSource birdsong;

    private GameObject currentBird;
    private AudioSource audiospec;
    private float originalVol;

    void Awake()
    {
        birdsong = GameObject.Find("AudioTestBirdsongs").GetComponent<AudioSource>();                                // this should be attached to the bird 
        audiospec = GameObject.Find("AudioSpectrum").GetComponent<AudioSource>();
        birdSpawner = GameObject.Find("BirdManager").GetComponent<BirdSpawner>();

        birdsong.volume = originalVol;
    }

    void Start()
    {
        state = currentState.hidden;
    }

    void Update()
    {
        updateState(state);
    }

    public void updateState(currentState state)                                                                      // bird states, player can interact with these states by using this function.
    {
        switch (state)
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
                birdSpawner.newBird = false;
                birdSpawner.NewBirdLocation();
                birdSpawner.BirdConstructor();
                state = currentState.hidden;
                break;
        }
    }
}
