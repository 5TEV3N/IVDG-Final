/* This script governs the audio (microphone) input: both the pitch detection and the measurement of the pitch against the correct notes of a provided birdsong.
 * 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // This adds some of the dictionary/indexing functionality I'm using


public class MicrophoneInput : MonoBehaviour {

	//	These variables wil eventually be used 
	public float volumeThreshold;
	//	public float volumeTrigger;
	public bool hummingMode;
	public bool easyMode;
	public float leniencyLength = 20.0f;
	public float leniencyPitch = 1;

	private string micDevice;
	public AudioSource micInput;
	private bool micStarted = false;

	// freqArray2048 is the frequencies of all "musical" pitches from E3 to B8, divided by 10.76660156 to exactly match their corresponding "slices" of the audio spectrum data when using the spectrum size of 2048.
	// If using a spectrum size of 4096, we can calculate pitches from E2 up. Probably overkill but may be useful for "easy" mode with humming (since we'll probably need the lower frequencies).
	// Old frequency array where numbers correspond to the frequency Hz itself! Kept here for posterity but we've changed to a different (simpler) counting system.
	// private int[] freqArray2048 = new int[] {24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100,101,102,103,104,105,106,107,108,109,110,111,112,113,114,115,116,117,118,119,120,121,122,123,124,125,126,127,128,129,130,131,132,133,134,135,136,137,138,139,140,141,142,143,144,145,146,147,148,149,150,151,152,153,154,155,156,157,158,159,160,161,162,163,164,165,166,167,168,169,170,171,172,173,174,175,176,177,178,179,180,181,182,183,184,185,186,187,188,189,190,191,192,193,194,195,196,197,198,199,200,201};

	// Old pitch array based on note names. Converted to number systems to make pitch detection easier.
	// private string[] oldPitchArray2048 = new string[] {"B3","C4","C4","C#4","C#4","D4","D#4","E4","E4","F4","F4","F#4","F#4","G4","G4","G#4","G#4","A4","A4","A#4","A#4","A#4","B4","B4","B4","C5","C5","C5","C#5","C#5","C#5","D5","D5","D5","D#5","D#5","D#5","E5","E5","E5","E5","F5","F5","F5","F5","F#5","F#5","F#5","G5","G5","G5","G5","G5","G#5","G#5","G#5","G#5","A5","A5","A5","A5","A5","A#5","A#5","A#5","A#5","A#5","B5","B5","B5","B5","B5","C6","C6","C6","C6","C6","C6","C#6","C#6","C#6","C#6","C#6","C#6","D6","D6","D6","D6","D6","D6","D6","D#6","D#6","D#6","D#6","D#6","D#6","E6","E6","E6","E6","E6","E6","E6","F6","F6","F6","F6","F6","F6","F6","F6","F#6","F#6","F#6","F#6","F#6","F#6","F#6","G6","G6","G6","G6","G6","G6","G6","G6","G6","G#6","G#6","G#6","G#6","G#6","G#6","G#6","G#6","G#6","A6","A6","A6","A6","A6","A6","A6","A6","A6","A6","A#6","A#6","A#6","A#6","A#6","A#6","A#6","A#6","A#6","B6","B6","B6","B6","B6","B6","B6","B6","B6","B6","B6","C7","C7","C7","C7","C7","C7","C7","C7","C7","C7","C7","C7"};
	private int[] pitchArray2048 = new int[] {0,1,1,2,2,3,4,5,5,5,5,6,6,7,7,8,8,9,9,10,10,10,11,11,11,12,12,12,13,13,13,14,14,14,15,15,15,16,16,16,16,17,17,17,17,18,18,18,19,19,19,19,19,20,20,20,20,21,21,21,21,21,22,22,22,22,22,23,23,23,23,23,24,24,24,24,24,24,25,25,25,25,25,25,26,26,26,26,26,26,26,27,27,27,27,27,27,28,28,28,28,28,28,28,29,29,29,29,29,29,29,29,30,30,30,30,30,30,30,31,31,31,31,31,31,31,31,31,32,32,32,32,32,32,32,32,32,33,33,33,33,33,33,33,33,33,33,34,34,34,34,34,34,34,34,34,35,35,35,35,35,35,35,35,35,35,35,36,36,36,36,36,36,36,36,36,36,36,36};
	private float[] spectrum;

	// No longer relevant, but I was using this spreadsheet tool to convert note -> frequency -> spectrum data slice here https://docs.google.com/spreadsheets/d/1_5y5PHcWEorZX10GDdnKLC4XBsPvsO6shfln-wiCwXQ/edit?usp=sharing
	// Simple HTML/JS tool to convert a pasted spreadsheet column (i.e. one entry per line) into a C# int[] or string[] https://suprko.github.io/csharp_text-to-array-parser/

	/* NOTE: regular Dictionaries are being used for the note/value pairs, but this is not best practices because apparently there is no guarantee that entries will remain ordered according to when they were added in. It seems to work for us.
	 * 
	 * If absolutely needed, we could convert the whole thing to an entirely new dictionary with:
	 * 	allNotes.Select((kvp, idx) => new {Index = idx, kvp.Key, kvp.Value})
	 * 
	 * Or redo the allNotes declaration to have index as key, sub-dictionary as value.
	 * This is deep end nice-to-have because it doesn't appear to affect gameplay at all, but would represent best practice.
	 */

	private Dictionary<int, int[]> allNotes; // Dictionary of all notes: KEY=note ID => VALUE=[lower freq bound, higher freq bound]
	private int numberOfNotes;
	private float[] notesTemplate;
	private float[] noteVolumes; // Array of all note volumes, including null or 0 entries: INDEX=note ID => VALUE=note volume
	private int[] notePeaks; // Array of "peak" notes; as in, noteVolumes with null and 0 entries removed: INDEX=noteID => VALUE=number of frames this note was dominant

	public bool listeningToPlayer = false;

	private GameObject UI;

	void Awake() {
		// Listing all Audio Input devices
		// foreach (string device in Microphone.devices) { Debug.Log(device); }

		// Setting microphone to the default first detected device for presentation & vernissage. Easy to make a menu option for player to decide.
		micDevice = Microphone.devices[0];

		micInput = gameObject.GetComponent<AudioSource> ();
	}

		
	void Start () {
		// Setting up pitch arrays. allNotes contains every pitch that can be detected in the game along with the range of frequencies represented by that note.
		// In allNotes, the key is the ID number of a note (simply counting from 0), and the value is an array in which the 0th entry is the lowest frequency spanned by that note, and the 1st value is the highest entry spanned by that note.
		// This is confusing right now but it's indispensible later.
		allNotes = new Dictionary<int, int[]> ();
		numberOfNotes = 0;
		 
		for (int i = 0; i < pitchArray2048.Length; i++) {
			if (!allNotes.ContainsKey (pitchArray2048 [i])) {
				int[] temparray = new int[2] { i + 23, i + 23 };
				allNotes.Add (pitchArray2048 [i], temparray);
				numberOfNotes++;
			} else {
				allNotes [pitchArray2048 [i]] [1] = i+23;
			}
		}

		// notesTemplate is an array of floats where the index of the array corresponds to the note ID, and the entry itself is 0.0f.
		// As its name suggests, this will serve as the "template" to which the noteVolumes array gets reset (inside FixedUpdate).
		notesTemplate = new float[numberOfNotes];
		for (int i = 0; i < numberOfNotes; i++) {
			notesTemplate [i] = 0.0f;
		}

		// notePeaks is initialized as an empty array with a size equal to the numberOfNotes that the game can measure.
		notePeaks = new int[numberOfNotes];

		// hummingMode will allow the player to hum the melody instead of whistling. Implemented but not fully reliable yet.
		// easyMode is on by default for now. Makes the pitch detection more lenient.
		hummingMode = false;
		easyMode = true;

		UI = GameObject.Find ("UI");
	}

	// This function is called every time the bird sings its song.
	public void SongStart () {
		// Reset the notePeaks every time there's a new song.
		notePeaks = new int[numberOfNotes];
		listeningToPlayer = true;
	}

	// This function is called a certain amount of time (defined in BirdAudioControl.cs) after the bird has sung its song, and declares the end of the "listening" period where the microphone is active.
	// It receives a dictionary called correctNotes from the BirdAudioControl script, which contains the correct notes and the duration they are held for in the current birdsong.
	public bool SongEnd (Dictionary<int, float> correctNotes) {
		listeningToPlayer = false;

		// Reset the variables that declare the song as correct or incorrect.
		bool whistleIsGood = false;
		int numberTotal = 0;
		int numberCorrect = 0;

		// Iterate through the correctNotes dictionary and compare its values with the values recorded from the microphone (all of that behaviour is inside the FixedUpdate function).
		foreach (int key in correctNotes.Keys) {
			numberTotal++;
			int thisKey = key;

			// hummingMode simply collapses the full range of notes into one octave, so singing the same melody at different octaves will produce the same result here.
			if (hummingMode) { thisKey = key % 12; }

			// In regular mode, each key-value pair in correctNotes is compared to the corresponding index-value pair in notePeaks.
			// leniencyLength determines how many frames of leniency is given to the note sung by the player. Currently if the player's whistled note is within 20 frames on either side of the correct duration, it is counted as correct, which is very generous I think.
			if (notePeaks [thisKey] != 0) {
				if (notePeaks [thisKey] > (correctNotes [key] - leniencyLength) && notePeaks [thisKey] < (correctNotes [key] + leniencyLength)) {
					numberCorrect++;

					// With easyMode enabled, each key-value pair in correctNotes is ALSO compared to the index-value pair in notePeaks one note up and one note down
				} else if (easyMode) {
					if (notePeaks [thisKey - 1] > (correctNotes [key] - leniencyLength) && notePeaks [thisKey - 1] < (correctNotes [key] + leniencyLength)) {
						numberCorrect++;
					} else if (notePeaks [thisKey + 1] > (correctNotes [key] - leniencyLength) && notePeaks [thisKey + 1] < (correctNotes [key] + leniencyLength)) {
						numberCorrect++;
					}
				}
			}
				
			// Printing results to console.
//			Debug.Log ("Correct notes : " + thisKey + ": " + correctNotes [key]);
		}


		// For debugging: print the notes that were sung/whistled to see if the pitch detection worked.
//		for (int i=0; i < notePeaks.Length; i++) {
//            if (notePeaks[i] != 0) {
//                Debug.Log("Sung notes : " + i + ": " + notePeaks[i]);
//            }
//		}

		// Compare numberCorrect with numberTotal (i.e. correctly sung notes vs number of actually correct notes). It's possible for numberCorrect to be greater than numberTotal because of easyMode.
		// Return whistleIsGood.
		if (numberCorrect >= numberTotal) {
			whistleIsGood = true;
		}
		return whistleIsGood;
	}

	// Pitch detection needs to be in FixedUpdate because it needs to be locked to IRL time.
	void FixedUpdate () {

		// Mic is actually always on because starting/stopping causes audio crackling and can lag.
		if (!micStarted) {
			micInput.clip = Microphone.Start (micDevice, true, 1, 44100);
			micInput.loop = true;

			// This line was found online. Supposed to cause a delay of (in this case) 100ms between when the mic starts recording and when its audio starts playing into the scene.
			// It seems that this no longer works in Unity 5, at least in this form.
//			while (!(Microphone.GetPosition(micDevice) > 100)) {}

			micInput.Play ();
			micStarted = true;
		}

		if (listeningToPlayer) {
			// HERE'S WHERE IT GETS REAL.
			// micInput gets processed by GetSpectrumData, which outputs an array ("spectrum") where the index is the ID of the "slice" of the frequencies and the value itself is the amplitude (volume) of that frequency slice.
			// 2048 is the size of the array, which means that the real audio spectrum (0-22050Hz) gets divided into 2048 slices, so each slice (as in, each entry in the array) represents a span of about 10.76660156 Hz. 
			spectrum = new float[2048];
			micInput.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

			// noteVolumes is a "translation" array that will take the frequency-volume data from spectrum, and condense it into note-volume data.
			noteVolumes = notesTemplate;
			for (int i=0; i < allNotes.Count; i++) {
				var note = allNotes.ElementAt (i);
				int lower = note.Value [0];
				int higher = note.Value [1];
				float volume = 0.0f;

				// Any frequency that is contained in the span covered by the current note gets added to the volume value of this note.
				for (int k = lower; k <= higher; k++) {
					volume += spectrum [k];
				}
				noteVolumes [note.Key] = volume;
			}

			// localPeaks takes noteVolumes and pulls out only the notes that represent a local peak (i.e. greater in volume than either neighbouring note).
			// Under the newer pitch detection system this step is in fact unnecessary, so it will be factored out for optimization purposes for the vernissage.
			Dictionary<int, float> localPeaks = new Dictionary<int, float> ();
			for (int i = 1; i < noteVolumes.Length - 1; i++) {
				if (noteVolumes[i] > 0.02 && noteVolumes[i] > noteVolumes[i - 1] && noteVolumes[i] > noteVolumes[i + 1]) {
					localPeaks.Add (i, noteVolumes[i]);
				}
			}

			// This block determines which note is the highest volume at this frame.
			int localMaxNote = -1;
			float localMaxVolume = 0.0f;
			int arrayLength = 0;
			foreach (int key in localPeaks.Keys) {
				if (localPeaks [key] > localMaxVolume) {
					localMaxNote = key;
					localMaxVolume = localPeaks [key];
				}
			}

			// Again, hummingMode collapses the full range of note values into one octave.
			if (hummingMode) {
				localMaxNote = localMaxNote % 12;
			}	

			// If localMaxNote changes at all from the default value of -1 (i.e. if any mic input was detected within the measured note range), notePeaks at this note value gets incremented by 1.
			// By the end of the song, notePeaks will thus contain an array of all 
			if (localMaxNote != -1) {
				notePeaks [localMaxNote]++;
			}
				
			// Push localMaxNote to UI (to light up the red note)
			UI.GetComponent<GameUI>().AudioHUDCurrentNote(localMaxNote);
		}

	}

}
