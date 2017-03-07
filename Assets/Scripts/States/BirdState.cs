using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdState : MonoBehaviour
{
<<<<<<< HEAD:Assets/Scripts/States/BirdState.cs
    public GameObject currentBirdSpawned;           // the current bird in the scene
    public AudioSource currentBirdCry;              // the cry of the current bird
    public Animator currentBirdAnimator;            // access the bird's animation

    void Awake()
    {
        currentBirdSpawned = GameObject.FindWithTag("Bird").gameObject;     // checks if there's a bird in the scene
        
            // this is subject to change, this is to get the information of the spawned bird
        //currentBirdCry = currentBirdSpawned.GetComponent<AudioSource>();
        //currentBirdAnimator = currentBirdSpawned.GetComponent<Animator>();        
=======
    // determines what state the bird is in
    public static string birdstate;

    void Start()
    {
        CurrentBirdState("hidden");
>>>>>>> stevenbranch:Assets/Scripts/Bird/BirdState.cs
    }

    public static string CurrentBirdState(string state)                     // bird states, player can interact with these states by using this function.
    {
        switch (state)
        {
            case "hidden":
                print("State: hidden. Bird is hidden and singing");
                break;
            case "birdcalls":
                print("State: bird calls. player must persude the bird with bird calls inorder to interact.");
                break;
            case "interacting":
                print("State: interacting. Bird is out of hiding and his in plain view to the player");
                break;
            case "flyaway":
                print("State: runaway. Bird flies away from the player because of reasons");
                break;
            default:
                print("Bird State is null");
                break;
        }
        return state;
    }
}
