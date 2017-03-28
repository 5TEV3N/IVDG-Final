using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerBirdController : MonoBehaviour
{
    BirdState myState;
    GameObject bird;

    void Awake()
    {
        myState = GameObject.Find("Bird").GetComponent<BirdState>();
        bird = GameObject.Find("TrailerBird");
    }

    void Update()
    {
        if (myState.state == BirdState.currentState.interacting)
        {
            bird.SetActive(true);
        }
        else
        {
            bird.SetActive(false);
        }
    }
}

