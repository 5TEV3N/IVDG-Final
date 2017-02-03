using UnityEngine;


[RequireComponent (typeof(AudioSource))]

public class AudioSpectrum : MonoBehaviour {	
	// Debug visualizer
//	void Update () {
//		float[] spectrum = new float[512];
//
//		AudioListener.GetSpectrumData( spectrum, 0, FFTWindow.Rectangular );
//
//		for( int i = 1; i < spectrum.Length-1; i++ )
//		{
//			Debug.DrawLine( new Vector3( i - 1, spectrum[i] + 10, 0 ), new Vector3( i, spectrum[i + 1] + 10, 0 ), Color.red );
//			Debug.DrawLine( new Vector3( i - 1, Mathf.Log( spectrum[i - 1] ) + 10, 2 ), new Vector3( i, Mathf.Log( spectrum[i] ) + 10, 2 ), Color.cyan );
//			Debug.DrawLine( new Vector3( Mathf.Log( i - 1 ), spectrum[i - 1] - 10, 1 ), new Vector3( Mathf.Log( i ), spectrum[i] - 10, 1 ), Color.green );
//			Debug.DrawLine( new Vector3( Mathf.Log( i - 1 ), Mathf.Log( spectrum[i - 1] ), 3 ), new Vector3( Mathf.Log( i ), Mathf.Log( spectrum[i] ), 3 ), Color.blue );
//		}
//	}

	public GameObject prefab;
	public int numBars = 64;
	public float radius = 1f;
	public GameObject[] bars;

	void Start () {
		for (int i=0; i < numBars; i++) {
			Vector3 position = new Vector3 (i*radius, 0, 0);
			Instantiate (prefab, position, Quaternion.identity);
		}
		bars = GameObject.FindGameObjectsWithTag ("audiobars");
	}



	void Update () {
		float[] spectrum = new float[numBars];

		AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

		for (int i = 0; i < numBars; i++) {
			Vector3 prevScale = bars[i].transform.localScale;
			prevScale.y = Mathf.Lerp (prevScale.y, spectrum [i] * 30, Time.deltaTime * 15);
			bars[i].transform.localScale = prevScale;
		}
	}
		
}