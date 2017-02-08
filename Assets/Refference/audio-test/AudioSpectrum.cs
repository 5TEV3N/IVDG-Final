using UnityEngine;


[RequireComponent (typeof(AudioSource))]

public class AudioSpectrum : MonoBehaviour {	

	public GameObject prefab;
	public GameObject[] bars;
	private float radius = 0.1f;
	private int[] freqArray = new int[] {13,14,15,16,17,18,19,20,21,22,24,25,27,28,30,32,33,35,38,40,42,45,47,50,53,56,60,63,67,71,75,80,84,89,95,100,106,113,119,126,134,142,150,159,169,179,189,200,212,225,238,253,268,284,300,318,337,357,378,401,425,450,477,505,535,567,601,636,674};

	void Start () {
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