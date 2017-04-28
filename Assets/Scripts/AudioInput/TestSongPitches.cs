/* This is the same as the MicrophoneInput script but just used to test the samples for their dominant notes */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // This adds some of the dictionary/indexing functionality I'm using


public class TestSongPitches : MonoBehaviour {

	public float volumeThreshold;

//	private string[] pitchArray2048 = new string[] {"B3","C4","C4","C#4","C#4","D4","D#4","E4","E4","F4","F4","F#4","F#4","G4","G4","G#4","G#4","A4","A4","A#4","A#4","A#4","B4","B4","B4","C5","C5","C5","C#5","C#5","C#5","D5","D5","D5","D#5","D#5","D#5","E5","E5","E5","E5","F5","F5","F5","F5","F#5","F#5","F#5","G5","G5","G5","G5","G5","G#5","G#5","G#5","G#5","A5","A5","A5","A5","A5","A#5","A#5","A#5","A#5","A#5","B5","B5","B5","B5","B5","C6","C6","C6","C6","C6","C6","C#6","C#6","C#6","C#6","C#6","C#6","D6","D6","D6","D6","D6","D6","D6","D#6","D#6","D#6","D#6","D#6","D#6","E6","E6","E6","E6","E6","E6","E6","F6","F6","F6","F6","F6","F6","F6","F6","F#6","F#6","F#6","F#6","F#6","F#6","F#6","G6","G6","G6","G6","G6","G6","G6","G6","G6","G#6","G#6","G#6","G#6","G#6","G#6","G#6","G#6","G#6","A6","A6","A6","A6","A6","A6","A6","A6","A6","A6","A#6","A#6","A#6","A#6","A#6","A#6","A#6","A#6","A#6","B6","B6","B6","B6","B6","B6","B6","B6","B6","B6","B6","C7","C7","C7","C7","C7","C7","C7","C7","C7","C7","C7","C7"};
	private int[] pitchArray2048 = new int[] {0,1,1,2,2,3,4,5,5,5,5,6,6,7,7,8,8,9,9,10,10,10,11,11,11,12,12,12,13,13,13,14,14,14,15,15,15,16,16,16,16,17,17,17,17,18,18,18,19,19,19,19,19,20,20,20,20,21,21,21,21,21,22,22,22,22,22,23,23,23,23,23,24,24,24,24,24,24,25,25,25,25,25,25,26,26,26,26,26,26,26,27,27,27,27,27,27,28,28,28,28,28,28,28,29,29,29,29,29,29,29,29,30,30,30,30,30,30,30,31,31,31,31,31,31,31,31,31,32,32,32,32,32,32,32,32,32,33,33,33,33,33,33,33,33,33,33,34,34,34,34,34,34,34,34,34,35,35,35,35,35,35,35,35,35,35,35,36,36,36,36,36,36,36,36,36,36,36,36};
	private float[] spectrum;

	// Spreadsheet tool to convert note -> frequency -> spectrum data slice here https://docs.google.com/spreadsheets/d/1_5y5PHcWEorZX10GDdnKLC4XBsPvsO6shfln-wiCwXQ/edit?usp=sharing
	// Simple HTML/JS tool to convert a pasted spreadsheet column (i.e. one entry per line) into a C# int[] or string[] https://suprko.github.io/csharp_text-to-array-parser/

	/* NOTE: regular Dictionaries being used, but apparently there is no guarantee that entries will remain ordered by the time they were added in? Seems to work for now, keep an eye on it.
	 * 
	 * Update: OK if needed, convert the whole thing to an entirely new dictionary with:
	 * 	allNotes.Select((kvp, idx) => new {Index = idx, kvp.Key, kvp.Value})
	 * 
	 * Or redo the allNotes declaration to have index as key, sub-dictionary as value.
	 * This will be ~very annoying~ to refactor everything so we'll see if it's necessary.
	 */

	private string micDevice;
	public AudioSource song;

	private Dictionary<int, int[]> allNotes; // Dictionary of all notes: KEY="note name" => VALUE=[lower freq bound, higher freq bound]
	private Dictionary<int, float> notesTemplate;
	private Dictionary<int, float> noteVolumes; // Dictionary of all note volumes: KEY="note name" => VALUE=note volume
	private Dictionary<int, int> notePeaks; // Dictionary of "peak" notes

	public bool listeningToPlayer = true;

	void Start () {
		song = GetComponent<AudioSource> ();

		// Setting up pitch arrays
		allNotes = new Dictionary<int, int[]> ();
		notesTemplate = new Dictionary<int, float> ();

		for (int i = 0; i < pitchArray2048.Length; i++) {
			if (!allNotes.ContainsKey (pitchArray2048 [i])) {
				int[] temparray = new int[2] { i + 23, i + 23 };
				allNotes.Add (pitchArray2048 [i], temparray);

				notesTemplate.Add (pitchArray2048 [i], 0);
			} else {
				allNotes [pitchArray2048 [i]] [1] = i+23;
			}
		}

		StartCoroutine (SongLoop ());
	}

	public IEnumerator SongLoop() {
		SongStart ();
		yield return new WaitForSeconds (3);
		SongEnd ();
	}

	void SongStart () {
		// Resetting the noteVolumes every time there's a new song.
		noteVolumes = notesTemplate;
		notePeaks = new Dictionary<int, int> ();

		song.Play ();

		listeningToPlayer = true;
	}

	void SongEnd () {
		listeningToPlayer = false;

		foreach (int key in notePeaks.Keys) {
			Debug.Log ("Song notes : " + key + ": " + notePeaks [key]);
		}
	}

	void FixedUpdate () {
		if (Input.GetKeyUp (KeyCode.P) && !listeningToPlayer) {
			SongStart ();
		} else if (Input.GetKeyUp(KeyCode.P) && listeningToPlayer) { 
			SongEnd ();
		}

		if (listeningToPlayer) {
			spectrum = new float[2048];
			song.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

			for (int i=0; i < allNotes.Count; i++) {
				var note = allNotes.ElementAt (i);
				int lower = note.Value [0];
				int higher = note.Value [1];
				float volume = 0.0f;

				for (int k = lower; k <= higher; k++) {
					volume += spectrum [k];
				}
				noteVolumes [note.Key] = volume;
			}

			Dictionary<int, float> localPeaks = new Dictionary<int, float> ();
			for (int i = 1; i < noteVolumes.Count - 1; i++) {
				var note = noteVolumes.ElementAt (i);
				float volume = note.Value;

				if (volume > 0.05 && volume > noteVolumes.ElementAt (i - 1).Value && volume > noteVolumes.ElementAt (i + 1).Value) {
					localPeaks.Add (note.Key, volume);
				}
			}

			int localMaxNote = -1;
			float localMaxVolume = 0.0f;
			foreach (int note in localPeaks.Keys) {
				if (localPeaks [note] > localMaxVolume) {
					localMaxNote = note;
					localMaxVolume = localPeaks [note];
				}
			}

			if (localMaxNote != -1 && !notePeaks.ContainsKey (localMaxNote)) {
				notePeaks.Add (localMaxNote, 1);	
			} else if (localMaxNote != -1 && notePeaks.ContainsKey(localMaxNote)) {
				notePeaks [localMaxNote] += 1;
			}
		}

	}

}
