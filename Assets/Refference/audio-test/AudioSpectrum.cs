/*	Elements of this script are adapted from an example provided in the Unity scripting API,
 * 	and a modified version of the same example used in a Youtube tutorial by "Orion" thedeveloperguy4517@gmail.com
 */

using UnityEngine;


[RequireComponent (typeof(AudioSource))]

public class AudioSpectrum : MonoBehaviour {	

	public GameObject prefab;
	public GameObject[] bars;

	private float radius = 0.1f;
	private int[] freqArray;

	// This is the frequencies of all "musical" pitches from E3 to B8, divided by 11.71875 to exactly match their corresponding "slices" of the audio spectrum data when using the spectrum size of 2048.
	private int[] freqArray2048 = new int[] {14,15,16,17,18,19,20,21,22,24,25,27,28,30,32,33,35,38,40,42,45,47,50,53,56,60,63,67,71,75,80,84,89,95,100,106,113,119,126,134,142,150,159,169,179,189,200,212,225,238,253,268,284,300,318,337,357,378,401,425,450,477,505,535,567,601,636,674};
	private string[] pitchArray2048 = new string[] {"E3","F3","F#3","G3","G#3","A3","A#3","B3","C4","C#4","D4","D#4","E4","F4","F#4","G4","G#4","A4","A#4","B4","C5","C#5","D5","D#5","E5","F5","F#5","G5","G#5","A5","A#5","B5","C6","C#6","D6","D#6","E6","F6","F#6","G6","G#6","A6","A#6","B6","C7","C#7","D7","D#7","E7","F7","F#7","G7","G#7","A7","A#7","B7","C8","C#8","D8","D#8","E8","F8","F#8","G8","G#8","A8","A#8","B8"};

	// Pitches from E2 to B8 if using a spectrum size of 4096
	private int[] freqArray4096 = new int[] {14,15,16,17,18,19,20,21,22,24,25,27,28,30,32,33,35,38,40,42,45,47,50,53,56,60,63,67,71,75,80,84,89,95,100,106,113,119,126,134,142,150,159,169,179,189,200,212,225,238,253,268,284,300,318,337,357,378,401,425,450,477,505,535,567,601,636,674,714,757,802,850,900,954,1010,1070,1134,1201,1273,1349};
	private string[] pitchArray4096 = new string[] {"E2","F2","F#2","G2","G#2","A2","A#2","B2","C3","C#3","D3","D#3","E3","F3","F#3","G3","G#3","A3","A#3","B3","C4","C#4","D4","D#4","E4","F4","F#4","G4","G#4","A4","A#4","B4","C5","C#5","D5","D#5","E5","F5","F#5","G5","G#5","A5","A#5","B5","C6","C#6","D6","D#6","E6","F6","F#6","G6","G#6","A6","A#6","B6","C7","C#7","D7","D#7","E7","F7","F#7","G7","G#7","A7","A#7","B7","C8","C#8","D8","D#8","E8","F8","F#8","G8","G#8","A8","A#8","B8"};

	// Tool to convert note -> frequency -> spectrum data slice here https://docs.google.com/spreadsheets/d/1_5y5PHcWEorZX10GDdnKLC4XBsPvsO6shfln-wiCwXQ/edit?usp=sharing
	// Paste column into this parser to quickly make it into an array ***link here***


	void Start () {
		freqArray = freqArray2048;

		for (int i=0; i < freqArray.Length; i++) {
			Vector3 position = new Vector3 (i*radius, 0, 0);
			Instantiate (prefab, position, Quaternion.identity);
		}
		bars = GameObject.FindGameObjectsWithTag ("audiobars");
	}

	void Update () {
		float[] spectrum = new float[2048];

		AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

		for (int i = 0; i < freqArray.Length; i++) {
			Vector3 prevScale = bars[i].transform.localScale;
			prevScale.y = Mathf.Lerp (prevScale.y, spectrum[freqArray[i]] * 30, Time.deltaTime * 30);
			bars[i].transform.localScale = prevScale;
		}
	}
		
}