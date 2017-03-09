using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdState : MonoBehaviour
{
    public enum BirdStates { hidden, birdcalls, interacting, flyaway };

    // determines what state the bird is in
    public BirdStates currentBirdState;

    private GameObject currentBird;
    public AudioSource birdsong;
    private AudioSource audiospec;
    private float originalVol;

    void Awake()
    {
        //currentBird = GameObject.FindGameObjectWithTag("Bird");
        birdsong = GetComponent<AudioSource>();
        audiospec = GameObject.Find("AudioSpectrum").GetComponent<AudioSource>();
        birdsong.volume = originalVol;
    }

    void Start()
    {
        currentBirdState = BirdStates.hidden;
       // currentBirdState = "hidden";
    }

    void Update()
    {
        updateState(currentBirdState);
    }

    public void updateState(BirdStates state)                                                                      // bird states, player can interact with these states by using this function.
    {
        switch (state)
        {
            case BirdStates.hidden:
                print("State: hidden. Bird is hidden and singing");
                birdsong.volume = Mathf.Lerp(birdsong.volume, originalVol, Time.deltaTime);
                audiospec.volume = Mathf.Lerp(birdsong.volume, originalVol, Time.deltaTime);
                break;
            case BirdStates.birdcalls:
                print("State: bird calls. player must persude the bird with bird calls inorder to interact.");
                birdsong.volume = Mathf.Lerp(birdsong.volume, 0.5f, Time.deltaTime);
                audiospec.volume = Mathf.Lerp(birdsong.volume, 0.5f, Time.deltaTime);

                AudioSpectrum.instance.SetClosestBird(this);

                break;
            case BirdStates.interacting:
                print("State: interacting. Bird is out of hiding and his in plain view to the player");
                break;
            case BirdStates.flyaway:
                print("State: runaway. Bird flies away from the player because of reasons");
                gameObject.GetComponent<BirdSpawner>().BirdConstructor();
                break;
        }
    }
}
