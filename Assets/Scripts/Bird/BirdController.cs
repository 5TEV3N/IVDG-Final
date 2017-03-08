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
    private AudioSource birdsong;
    private AudioSource audiospec;
    private float originalVol;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        discovered = GameObject.Find("BirdNameDisplay").GetComponent<Text>();
        birdsong = GameObject.Find("AudioTestBirdsongs").GetComponent<AudioSource>();
        audiospec = GameObject.Find("AudioSpectrum").GetComponent<AudioSource>();
        birdsong.volume = originalVol;
    }

    void Update()
    {
        birdDistance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        birdName = BirdSpawner.currentBirdName;

        if (birdDistance <= 4.5)
        {
            if (birdDistance > 6)
            {
                //BirdState.CurrentBirdState("runaway");
            }
            else
            {
                //BirdState.CurrentBirdState("birdcalls");
                discovered.text = "Discovered a\n " + birdName;
                birdsong.volume = Mathf.Lerp(0.5f, originalVol, Time.deltaTime * 5f);
                audiospec.volume = Mathf.Lerp(0.5f, originalVol, Time.deltaTime * 5f);
            }
        }
        else
        {
            discovered.text = "";
            birdsong.volume = Mathf.Lerp(originalVol, 0.5f, Time.deltaTime * 5f);
            audiospec.volume = Mathf.Lerp(originalVol, 0.5f, Time.deltaTime * 5f);
        }

        if (BirdState.successfullBirdCall == true)
        {

            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(0, 0, 0), Time.deltaTime);
        }
    }

}
//      Logic
// if player reaches a certain distance, Bird State changes from hidden to birdc. if the player reaches that distance but leaves at a certain threshold, Bird state changes from hidden to runaway.
