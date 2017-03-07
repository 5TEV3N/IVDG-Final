using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdState : MonoBehaviour
{
    // determines what state the bird is in
    // public static string birdstate;
    public static bool successfullBirdCall;             //prototype test
    void Start()
    {
        CurrentBirdState("hidden");
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
