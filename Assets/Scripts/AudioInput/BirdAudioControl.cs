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

	private Coroutine newCoroutine;
	private float timer;
	public float songLength;
	public int birdWait;

	public int birdDifficulty; // Determines what songs bird chooses from, how accurate song needs to be, and how many attempts are required.
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

		Initialize ();
	}

	public void Initialize() {
		birdSuccess = false;
		birdFailure = false;

		// Randomize later
		birdDifficulty = 0; // Three levels? 0,1,2?
		failsRemaining = 3 - birdDifficulty;
		successNeeded = 3 + birdDifficulty;
		successCurrent = 0;

		// Pulls bird song and corresponding "correct pitches" dictionary from the AllSongs script (randomize later, organize based on bird difficulty)
		birdSong.clip = allSongs.GetComponent<AllSongs>().listOfSongs[1];
		correctNotes = allSongs.GetComponent<AllSongs>().songPitches[1];

		// Tie bird waiting time between songs to song length
		songLength = 0.0f;
		foreach (int key in correctNotes.Keys) {
			songLength += correctNotes [key];
		}
		songLength = songLength / 60;
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

	// SingAndListenToPlayer and StopListening functions that in turn call SongStart and SongEnd functions in the MicrophoneInput script, to start/stop recording and check whistling accuracy
	// SingLoop includes both of these functions, with StopListening invoked on a timer
	public void SingLoop() {
		if (!audioUIExists) {
			AudioUIControl ("build");
		}
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

		whistleIsGood = audioManager.GetComponent<MicrophoneInput> ().SongEnd (correctNotes);

		if (whistleIsGood) {
			successCurrent += 1;	
			if (successCurrent == successNeeded) {
				birdSuccess = true;
				AudioUIControl ("hide");
			}
		} else { 
			failsRemaining -= 1;
			if (failsRemaining == 0) {
				birdFailure = true;
				AudioUIControl ("hide");
			}
		}

		Debug.Log ("successCurrent : " + successCurrent + ", failsRemaining : " + failsRemaining);
	}
		

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
	}

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