using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsLoops : MonoBehaviour {

	public AudioClip[] listOfFootsteps;
	private AudioSource audioSource;

	void Start() {
		audioSource = this.GetComponent<AudioSource> ();
		audioSource.clip = listOfFootsteps[Random.Range(0, 2)];
		audioSource.loop = true;
	}

	public void FootstepsStart() {
		audioSource.Play ();
	}

	public void FootstepsStop() {
		audioSource.Stop ();
		this.GetComponent<AudioSource>().clip = listOfFootsteps[Random.Range(0, 2)];
	}
}
