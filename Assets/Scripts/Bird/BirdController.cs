using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    //Script that controls the bird directly. 
    public string birdName;
    private GameObject player;
    [Header("Dubg")]
    public float birdDistance;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        birdDistance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        birdName = BirdSpawner.currentBirdName;

        if (birdDistance <= 4.5)
        {
            BirdState.CurrentBirdState("birdcalls");
            /*
            if ()
            {

            }
            */
        }
        //else if( certain threshold is left, then hidden to runaway)
    }
}
//      Logic
// if player reaches a certain distance, Bird State changes from hidden to birdc. if the player reaches that distance but leaves at a certain threshold, Bird state changes from hidden to runaway.
