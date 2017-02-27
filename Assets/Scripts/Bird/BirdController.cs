using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdController : MonoBehaviour
{
    //Script that controls the bird directly. 
    [Header("Debug")]
    public float birdDistance;
    public string birdName;

    private Text discovered;
    private GameObject player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        discovered = GameObject.Find("BirdNameDisplay").GetComponent<Text>();
    }

    void Update()
    {
        birdDistance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        birdName = BirdSpawner.currentBirdName;

        if (birdDistance <= 4.5)
        {
            if (birdDistance > 6)
            {
                BirdState.CurrentBirdState("runaway");
            }
            else
            {
                BirdState.CurrentBirdState("birdcalls");
                discovered.text = "Discovered a\n " + birdName;
            }
        }
        else
        {
            discovered.text = "";
        }
    }

}
//      Logic
// if player reaches a certain distance, Bird State changes from hidden to birdc. if the player reaches that distance but leaves at a certain threshold, Bird state changes from hidden to runaway.
