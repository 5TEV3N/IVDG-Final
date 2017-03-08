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
        
        //TESTING
        birdsong = GameObject.Find("AudioTestBirdsongs").GetComponent<AudioSource>();
        audiospec = GameObject.Find("AudioSpectrum").GetComponent<AudioSource>();
        birdsong.volume = originalVol;
    }

    void Update()
    {
        birdDistance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        birdName = BirdSpawner.currentBirdName;

        if (birdDistance <= 8)
        {
            if (birdDistance > 10)
            {
                //BirdState.CurrentBirdState("runaway");
            }
            else
            {
                //BirdState.CurrentBirdState("birdcalls");
                discovered.text = "Discovered a\n " + birdName;
                birdsong.volume = Mathf.Lerp(birdsong.volume, 0.5f, Time.deltaTime);
                audiospec.volume = Mathf.Lerp(birdsong.volume, 0.5f, Time.deltaTime);
            }
        }
        else
        {
            birdsong.volume = Mathf.Lerp(birdsong.volume, originalVol, Time.deltaTime);
            audiospec.volume = Mathf.Lerp(birdsong.volume, originalVol, Time.deltaTime);
            discovered.text = "";
        }

        if (BirdState.successfullBirdCall == true)
        {

            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(0, 0, 0), Time.deltaTime);
        }
    }

}