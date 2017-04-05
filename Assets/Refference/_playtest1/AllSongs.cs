/* This script is just a container for all of the bird songs.
 * Every element in this script is necessarily placed manually. So things need to stay in a specific order for it to work.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSongs : MonoBehaviour {

	public AudioClip[] listOfSongs;
	public Dictionary<string, float>[] songPitches;

	void Awake() {
		songPitches = new Dictionary<string, float>[] {
			new Dictionary<string, float> (),
			new Dictionary<string, float> ()
		};

		songPitches [0].Add ("B6", 40.0f);
		songPitches [0].Add ("D6", 26.0f);

		songPitches [1].Add ("C5", 20.0f);
	}
}
