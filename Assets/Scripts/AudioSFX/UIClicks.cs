﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClicks : MonoBehaviour {

	public AudioClip[] listOfClicks;
	private AudioSource audioSource;

	void Start () {
		audioSource = this.GetComponent<AudioSource> ();
		//audioSource.clip = listOfClicks[Random.Range(0, 4)];
		audioSource.loop = false;
	}
		
	public void UIClick() {
        audioSource.clip = listOfClicks[Random.Range(0, 4)];
        audioSource.Play ();
	}
}
