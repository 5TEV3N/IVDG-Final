using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsLoops : MonoBehaviour {

	public AudioClip[] listOfFootsteps;
	private AudioSource audioSource;

	void Start() {
		audioSource = this.GetComponent<AudioSource> ();
		audioSource.clip = listOfFootsteps[Random.Range(0,3)];
		audioSource.loop = true;
	}

	// Making play/stop functions easily accessible to other scripts.
	public void FootstepsStart() {
		audioSource.Play ();
	}
	public void FootstepsStop() {
		audioSource.Stop ();
		this.GetComponent<AudioSource>().clip = listOfFootsteps[Random.Range(0, 3)]; // Every time the footsteps are stopped, a random footstep audio loop is selected for the next time.
	}
}
