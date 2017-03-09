using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(BirdState))]
public class BirdController : MonoBehaviour
{
    [Header("Debug")]
    public float birdDistance;
    public string birdName;

    private BirdState myState;

    private Text discovered;
    private GameObject player;

    void Awake()
    {
        myState = GetComponent<BirdState>();
        player = GameObject.FindGameObjectWithTag("Player");
        discovered = GameObject.Find("BirdNameDisplay").GetComponent<Text>();
    }

    void Update()
    {
        birdDistance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        birdName = BirdSpawner.currentBirdName;

        if (birdDistance <= 8)
        {
            if (myState.currentBirdState != BirdState.BirdStates.interacting)
            {
                myState.currentBirdState = BirdState.BirdStates.birdcalls;
                discovered.text = "Discovered a\n " + birdName;
            }
        }

        else if (birdDistance >= 13)
        {
            if (myState.currentBirdState != BirdState.BirdStates.interacting)
            {
                myState.currentBirdState = BirdState.BirdStates.hidden;
                discovered.text = "";
            }
        }

        if (myState.currentBirdState == BirdState.BirdStates.interacting)
        {
            discovered.text = "";
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(0, 0, 0), Time.deltaTime);
            StartCoroutine("TimeToInteract");
        }
    }
    
    IEnumerable TimeToInteract()
    {
        yield return new WaitForSeconds(1f);
        myState.currentBirdState = BirdState.BirdStates.flyaway;
    }
}