/* This script is just a container for all of the bird songs.
 * Every element in this script is necessarily placed manually. So things need to stay in a specific order for it to work.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSongs : MonoBehaviour {

	public AudioClip[] listOfSongs;
	public Dictionary<int, float>[] songPitches;

	void Awake() {
		songPitches = new Dictionary<int, float>[] {
			new Dictionary<int, float> (),
			new Dictionary<int, float> ()
		};

		songPitches [0].Add (23, 34.0f);
		songPitches [0].Add (26, 26.0f);

		songPitches [1].Add (12, 20.0f);
	}
}
