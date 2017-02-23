using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdState : MonoBehaviour
{
    // determines what state the bird is in

    public string birdState;
    public static string CurrentBirdState(string state)                     // bird states, player can interact with these states by using this function.
    {
        switch (state)
        {
            case "hidden":
                print("State: hidden. Bird is hidden and singing");
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
