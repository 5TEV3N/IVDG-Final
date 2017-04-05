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
	public bool whistleIsGood;
	public int birdWait;

	public int birdDifficulty; // Determines what songs bird chooses from, how accurate song needs to be, and how many attempts are required.
	public int attemptsRemaining;
	public int successNeeded;
	public int successCurrent;

	public bool birdSuccess;
	public bool birdFailure;

	public Dictionary<string, float> correctNotes;

	public AudioSource birdSong;
	public GameObject allSongs;
	public GameObject audioManager;

	// TESTING VARIABLES
	private bool testingOn;

	void Start() {
		testingOn = false;

		birdSong = GetComponent<AudioSource> ();

		allSongs = GameObject.Find ("AllSongs");
		audioManager = GameObject.Find ("AudioManager");

		Initialize ();
	}

	public void Initialize() {
		birdSuccess = false;
		birdFailure = false;

		// Randomize later
		birdDifficulty = 0;
		attemptsRemaining = 5 - birdDifficulty;
		successNeeded = 3 + birdDifficulty;
		successCurrent = 0;

		// Pulls bird song and corresponding "correct pitches" dictionary from the AllSongs script (randomize later)
		birdSong.clip = allSongs.GetComponent<AllSongs>().listOfSongs[0];
		correctNotes = allSongs.GetComponent<AllSongs>().songPitches[0];

		// Tie to song length
		birdWait = 5;
	}

	// Sing and StopSinging functions that in turn call SongStart and SongEnd functions in the MicrophoneInput script, to start/stop recording and check whistling accuracy

	public IEnumerator SingLoop() {
		testingOn = true;
		SingAndListenToPlayer ();
		yield return new WaitForSeconds (birdWait);
		StopListening ();
		testingOn = false;

		if (whistleIsGood) {
			successCurrent += 1;	
			if (successCurrent == successNeeded) {
				birdSuccess = true;
			}
		} else { 
			attemptsRemaining -= 1;
			if (attemptsRemaining == 0) {
				birdFailure = true;
			}
		}

		Debug.Log ("successCurrent : " + successCurrent + ", attemptsRemaining : " + attemptsRemaining);
	}

	void SingAndListenToPlayer () {
		birdSong.Play ();
		whistleIsGood = false;
		audioManager.GetComponent<MicrophoneInput> ().SongStart ();
	}

	void StopListening() {
		whistleIsGood = audioManager.GetComponent<MicrophoneInput> ().SongEnd (correctNotes);

		foreach (string key in correctNotes.Keys) {
			Debug.Log ("Correct notes : " + key + ": " + correctNotes [key]);
		}
	}

	void Update () {
		// Using space key as universal "play song" button for testing
		if (Input.GetKeyUp (KeyCode.Space) && !testingOn) {
			StartCoroutine (SingLoop ());
		}
	}

}