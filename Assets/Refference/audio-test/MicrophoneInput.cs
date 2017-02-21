using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneInput : MonoBehaviour {

	private string micDevice;
	public AudioSource micInput;


	void Awake() {
		foreach (string device in Microphone.devices) {
			Debug.Log(device);
		}

		// Setting microphone to the default first detected device for now
		micDevice = Microphone.devices[0];

		// Right now just starts recording right away for 999 seconds.
		// Later wrap this in a function that is called at relevant points.
		micInput = gameObject.GetComponent<AudioSource> ();
		micInput.clip = Microphone.Start (micDevice, true, 5, 44100);
		micInput.loop = true;

		while (!(Microphone.GetPosition(null) > 500)) {}
		micInput.Play ();

	}
		
	
	// Update is called once per frame
	void Update () {
		
	}
}
