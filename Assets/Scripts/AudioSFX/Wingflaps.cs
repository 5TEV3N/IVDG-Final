using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wingflaps : MonoBehaviour {

	public AudioClip[] listOfWingflaps;
	private AudioSource audioSource;

	void Start () {
		audioSource = this.GetComponent<AudioSource> ();
		audioSource.clip = listOfWingflaps[Random.Range(0, 1)];
		audioSource.loop = false;
	}

	public void FlapPlay() {
		audioSource.Play ();
		audioSource.clip = listOfWingflaps[Random.Range(0, 1)];
	}

}
