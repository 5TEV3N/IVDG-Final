/* Due to each GameObject only being able to hold one AudioSource,
 * the audio measurement itself has been moved to MicrophoneInput.cs (on the AudioManager empty object)
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // This adds some of the dictionary/indexing functionality I'm using

[RequireComponent(typeof(AudioSource))]

public class BirdAudioControl: MonoBehaviour {	

	// Bools that get passed to other scripts to be tied to in-game events
	public bool playerInRange;
	public bool whistleIsGood;

	// Floats for the length of the song, which contributes to how long the bird waits between singing cycles
	public float songLength;
	public int birdWait;

	// Not in place right now, but exists if we want to scale difficulty.
	// Determines what song to select from and how many successful whistles required or failures allowed
	public int birdDifficulty;

	// Variables for the progression of the birdsong loop: how many successes/failures the player has done, how many remaining, and ultimately whether to return success or failure to the BirdState script.
	public int failsRemaining;
	public int successNeeded;
	public int successCurrent;
	public bool birdSuccess;
	public bool birdFailure;

	public Dictionary<int, float> correctNotes;
	public int[] correctNotesArray;

	public AudioSource birdSong;
	public GameObject allSongs;
	public GameObject audioManager;

	public bool birdSingingOn;

	private GameObject UI;
	private bool audioUIExists;

	void Start() {
		birdSingingOn = false;

		birdSong = GetComponent<AudioSource> ();
		birdSong.volume = 0.5f;

		allSongs = GameObject.Find ("AllSongs");
		audioManager = GameObject.Find ("AudioManager");

		UI = GameObject.Find ("UI");
		audioUIExists = false;

		// Initialize is separated out into a separate function so that it can be called at will by the BirdState script.
		// This is due to the fact that we actually only have one bird in the scene, which simply gets moved around. So it needs to be reinitialized as a "new" bird each time.
		Initialize ();
	}

	public void Initialize() {
		birdSuccess = false;
		birdFailure = false;

		// Variables in place so we can randomize things later, maybe scale up difficulty with successful bird calls.
		birdDifficulty = 0; // Three levels? 0,1,2?
		failsRemaining = 3 - birdDifficulty;
		successNeeded = 3 + birdDifficulty;
		successCurrent = 0;

		// Pulls random bird song and corresponding "correct pitches" dictionary from the AllSongs script
		int thisSong = Random.Range(0, 8);

		birdSong.clip = allSongs.GetComponent<AllSongs>().listOfSongs[thisSong];
		correctNotes = allSongs.GetComponent<AllSongs>().songPitches[thisSong];

		// Tie bird waiting time between songs to song length (measured in frames, not seconds!)
		songLength = 0.0f;
		foreach (int key in correctNotes.Keys) {
			songLength += correctNotes [key];
		}
		songLength = songLength / 60; // converting frames to seconds
		songLength = Mathf.Round (songLength);
		int songLengthInt = (int)songLength;
		birdWait = 4 + songLengthInt;

		// Push all correctNotes into a new array to be easily accessed by UI
		var correctNotesList = new List<int>();
		foreach (int key in correctNotes.Keys) {
			correctNotesList.Add (key);
		}
		correctNotesArray = new int[correctNotes.Count];
		correctNotesArray = correctNotesList.ToArray ();
	}

	// SingAndListenToPlayer and StopListening functions that in turn call SongStart and SongEnd functions in the MicrophoneInput script, to start/stop recording AND check whistling accuracy
	// SingLoop includes both of these functions, with StopListening invoked on a timer
	public void SingLoop() {
		// Make the audio UI appear if it doesn't already
		if (!audioUIExists) { AudioUIControl ("build"); }

		// I read that, for this purpose, using Invoke is a simpler way to delay the firing of a function than using a coroutine.
		SingAndListenToPlayer ();
		Invoke ("StopListening", Random.Range(birdWait, birdWait + 3));
	}

	void SingAndListenToPlayer () {
		birdSingingOn = true;

		birdSong.Play ();
		whistleIsGood = false;
		audioManager.GetComponent<MicrophoneInput> ().SongStart ();
	}

	void StopListening() {
		birdSingingOn = false;

		// Pull the whistleIsGood value from the MicrophoneInput script
		whistleIsGood = audioManager.GetComponent<MicrophoneInput> ().SongEnd (correctNotes);

		if (whistleIsGood) {
			successCurrent += 1; // increment current success count
            UI.GetComponent<GameUI>().SuccessBirdCallIcons(successCurrent);
            if (successCurrent == successNeeded) { // if current success count matches success needed, this bool will end the bird singing loop
				birdSuccess = true;
				AudioUIControl ("hide");
			}
		} else { 
			failsRemaining -= 1; // deincrement current failure-remaining count
            UI.GetComponent<GameUI>().FailedBirdCallIcons(failsRemaining);
			if (failsRemaining == 0) {
				birdFailure = true;
				AudioUIControl ("hide");
			}
		}
	}
		

	/* TESTING BLOCK
	void Update () {
		// Use 1 key to force success, 2 key to force failure
		if (Input.GetKeyUp (KeyCode.Alpha1)) {
			birdSuccess = true;
			AudioUIControl ("hide");
		}
		if (Input.GetKeyUp (KeyCode.Alpha2)) {
			birdFailure = true;
			AudioUIControl ("hide");
		}
	} */

	public void AudioUIControl(string instruction) {
		switch (instruction) {
		case "build":
			UI.GetComponent<GameUI> ().AudioHUDSetup ();
			audioUIExists = true;
			break;
		case "hide":
			UI.GetComponent<GameUI> ().AudioHUDClear ();
			audioUIExists = false;
			break;
		}
	}

}