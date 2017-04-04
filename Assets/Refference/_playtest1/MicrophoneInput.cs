using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // This adds some of the dictionary/indexing functionality I'm using


public class MicrophoneInput : MonoBehaviour {

	//	Gonna use these later to set volume thresholds based on a user mic test
	public float volumeThreshold;
	//	public float volumeTrigger;

	// This is the frequencies of all "musical" pitches from E3 to B8, divided by 11.71875 to exactly match their corresponding "slices" of the audio spectrum data when using the spectrum size of 2048.
	// Can calculate pitches from E2 up if using a spectrum size of 4096. Probably overkill? Revisit for "easy" mode with humming (will need the lower frequencies).
	// Don't need frequency array with new counting system, since it's just all integers in a row starting from 24.
	//	private int[] freqArray2048 = new int[] {24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100,101,102,103,104,105,106,107,108,109,110,111,112,113,114,115,116,117,118,119,120,121,122,123,124,125,126,127,128,129,130,131,132,133,134,135,136,137,138,139,140,141,142,143,144,145,146,147,148,149,150,151,152,153,154,155,156,157,158,159,160,161,162,163,164,165,166,167,168,169,170,171,172,173,174,175,176,177,178,179,180,181,182,183,184,185,186,187,188,189,190,191,192,193,194,195,196,197,198,199,200,201};
	private string[] pitchArray2048 = new string[] {"B3","C4","C4","C#4","C#4","D4","D#4","E4","E4","F4","F4","F#4","F#4","G4","G4","G#4","G#4","A4","A4","A#4","A#4","A#4","B4","B4","B4","C5","C5","C5","C#5","C#5","C#5","D5","D5","D5","D#5","D#5","D#5","E5","E5","E5","E5","F5","F5","F5","F5","F#5","F#5","F#5","G5","G5","G5","G5","G5","G#5","G#5","G#5","G#5","A5","A5","A5","A5","A5","A#5","A#5","A#5","A#5","A#5","B5","B5","B5","B5","B5","C6","C6","C6","C6","C6","C6","C#6","C#6","C#6","C#6","C#6","C#6","D6","D6","D6","D6","D6","D6","D6","D#6","D#6","D#6","D#6","D#6","D#6","E6","E6","E6","E6","E6","E6","E6","F6","F6","F6","F6","F6","F6","F6","F6","F#6","F#6","F#6","F#6","F#6","F#6","F#6","G6","G6","G6","G6","G6","G6","G6","G6","G6","G#6","G#6","G#6","G#6","G#6","G#6","G#6","G#6","G#6","A6","A6","A6","A6","A6","A6","A6","A6","A6","A6","A#6","A#6","A#6","A#6","A#6","A#6","A#6","A#6","A#6","B6","B6","B6","B6","B6","B6","B6","B6","B6","B6","B6","C7","C7","C7","C7","C7","C7","C7","C7","C7","C7","C7","C7"};
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
	public AudioSource micInput;

	private Dictionary<string, int[]> allNotes; // Dictionary of all notes: KEY="note name" => VALUE=[lower freq bound, higher freq bound]
	private Dictionary<string, float> notesTemplate;
	private Dictionary<string, float> noteVolumes; // Dictionary of all note volumes: KEY="note name" => VALUE=note volume
	private Dictionary<string, int> notePeaks; // Dictionary of "peak" notes

	public bool listeningToPlayer = false;

	void Awake() {
		// Listing all Audio Input devices
		foreach (string device in Microphone.devices) { Debug.Log(device); }

		// Setting microphone to the default first detected device for now
		// Can make menu option to decide this later
		micDevice = Microphone.devices[0];

		// Right now just starts recording right away for 999 seconds.
		// Later wrap this in a function that is called at relevant points.
		micInput = gameObject.GetComponent<AudioSource> ();

		micInput.clip = Microphone.Start (micDevice, true, 5, 44100);
		micInput.loop = true;

		while (!(Microphone.GetPosition(null) > 500)) {}
		micInput.Play ();
	}

	void Start () {
		// Setting up pitch arrays
		allNotes = new Dictionary<string, int[]> ();
		notesTemplate = new Dictionary<string, float> ();

		for (int i = 0; i < pitchArray2048.Length; i++) {
			if (!allNotes.ContainsKey (pitchArray2048 [i])) {
				int[] temparray = new int[2] { i + 23, i + 23 };
				allNotes.Add (pitchArray2048 [i], temparray);

				notesTemplate.Add (pitchArray2048 [i], 0);
			} else {
				allNotes [pitchArray2048 [i]] [1] = i+23;
			}
		}
	}

	public void SongStart () {
		// Resetting the noteVolumes every time there's a new song.
		noteVolumes = notesTemplate;
		notePeaks = new Dictionary<string, int> ();

		listeningToPlayer = true;
	}

	public bool SongEnd (Dictionary<string,float> correctNotes) {
		listeningToPlayer = false;

		//// EDIT THIS
		bool whistleIsGood = false;

		foreach (string key in correctNotes.Keys) {
			if (notePeaks.ContainsKey (key)) {
				if (notePeaks [key] > (correctNotes [key] - correctNotes [key] * 0.20) && notePeaks [key] < (correctNotes [key] + correctNotes [key] * 0.20)) {
					whistleIsGood = true;
				}
			} else {
				whistleIsGood = false;
			}
		}

		Debug.Log (whistleIsGood);

		foreach (string key in notePeaks.Keys) {
			Debug.Log ("Sung notes : " + key + ": " + notePeaks [key]);
		}

		return whistleIsGood;
	}

	void Update () {
		if (listeningToPlayer) {
			spectrum = new float[2048];
			micInput.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

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

			Dictionary<string, float> localPeaks = new Dictionary<string, float> ();
			for (int i = 1; i < noteVolumes.Count - 1; i++) {
				var note = noteVolumes.ElementAt (i);
				float volume = note.Value;

				if (volume > 0.05 && volume > noteVolumes.ElementAt (i - 1).Value && volume > noteVolumes.ElementAt (i + 1).Value) {
					localPeaks.Add (note.Key, volume);
				}
			}

			string localMaxNote = "";
			float localMaxVolume = 0.0f;
			foreach (string note in localPeaks.Keys) {
				if (localPeaks [note] > localMaxVolume) {
					localMaxNote = note;
					localMaxVolume = localPeaks [note];
				}
			}

			if (localMaxNote != "" && !notePeaks.ContainsKey (localMaxNote)) {
				notePeaks.Add (localMaxNote, 1);	
			} else if (localMaxNote != "" && notePeaks.ContainsKey(localMaxNote)) {
				notePeaks [localMaxNote] += 1;
			}
		}

	}

}
