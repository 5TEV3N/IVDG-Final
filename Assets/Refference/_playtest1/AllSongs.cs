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
			new Dictionary<int, float> (),
			new Dictionary<int, float> (),
			new Dictionary<int, float> (),
			new Dictionary<int, float> (),
			new Dictionary<int, float> (),
			new Dictionary<int, float> (),
			new Dictionary<int, float> (),
			new Dictionary<int, float> ()
		};

		// birdsong-easy-dingdong
		songPitches [0].Add (20, 39.0f);
		songPitches [0].Add (23, 21.0f);

		// birdsong-easy-single
		songPitches [1].Add (24, 33.0f);

		// birdsong-easy-slide
		songPitches [2].Add (22, 10.0f);
		songPitches [2].Add (23, 49.0f);

		// birdsong-medium-ascending
		songPitches [3].Add (20, 40.0f);
		songPitches [3].Add (25, 2.0f);
		songPitches [3].Add (27, 22.0f);

		// birdsong-medium-dingdong
		songPitches [4].Add (19, 21.0f);
		songPitches [4].Add (23, 28.0f);
		songPitches [4].Add (26, 23.0f);

		// birdsong-medium-punch
		songPitches [5].Add (24, 10.0f);
		songPitches [5].Add (25, 10.0f);

		// birdsong-hard-ascending
		songPitches [6].Add (24, 23.0f);
		songPitches [6].Add (25, 11.0f);
		songPitches [6].Add (27, 10.0f);

		// birdsong-hard-descending
		songPitches [7].Add (23, 17.0f);
		songPitches [7].Add (24, 6.0f);
		songPitches [7].Add (29, 6.0f);
		songPitches [7].Add (30, 10.0f);

		// birdsong-hard-slide
		songPitches [8].Add (21, 9.0f);
		songPitches [8].Add (26, 24.0f);
		songPitches [8].Add (27, 18.0f);
	}
}
