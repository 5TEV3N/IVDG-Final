/* Due to each GameObject only being able to hold one AudioSource,
 * the audio measurement itself has been moved to MicrophoneInput.cs (on the AudioManager empty object)
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // This adds some of the dictionary/indexing functionality I'm using

[RequireComponent(typeof(AudioSource))]

public class BirdAudioControl: MonoBehaviour {	

	public bool playerInRange;

	public int birdDifficulty; // Determines what songs bird chooses from, how accurate song needs to be, and how many attempts are required.
	public Dictionary<string, float> correctNotes;

	public AudioSource birdSong;
	public GameObject allSongs;
	public GameObject audioManager;

	// TESTING VARIABLES
	private bool testingOn;

	void Start () {
		// Randomize later
		birdDifficulty = 0;
		birdSong = GetComponent<AudioSource> ();

		allSongs = GameObject.Find ("AllSongs");
		audioManager = GameObject.Find ("AudioManager");

		// Pulls bird song and corresponding "correct pitches" dictionary from the AllSongs script (randomize later)
		birdSong.clip = allSongs.GetComponent<AllSongs>().listOfSongs[birdDifficulty];
		correctNotes = allSongs.GetComponent<AllSongs>().songPitches[birdDifficulty];
	}

	void Update () {
		// Using space key as universal "play song"
		if (Input.GetKeyUp (KeyCode.Space) && !testingOn) {
			Sing ();
			testingOn = true;
		} else if (Input.GetKeyUp (KeyCode.Space) && testingOn) {
			StopSinging ();
			testingOn = false;
		}

	}

	// Sing and StopSinging functions that in turn call SongStart and SongEnd functions in the MicrophoneInput script, to start/stop recording and check whistling accuracy
	void Sing () {
		birdSong.Play ();
		audioManager.GetComponent<MicrophoneInput> ().SongStart ();
	}

	void StopSinging() {
		audioManager.GetComponent<MicrophoneInput> ().SongEnd (correctNotes);
		foreach (string key in correctNotes.Keys) {
			Debug.Log ("Correct notes : " + key + ": " + correctNotes [key]);
		}
	}

}