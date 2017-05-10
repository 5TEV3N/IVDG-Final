/* This script is just a container for all of the bird songs.
 * Every element in this script is necessarily placed manually. So things need to stay in a specific order for it to work.
 * For our purposes and the scale of this game, it didn't seem worth our time to automate this process
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSongs : MonoBehaviour {

	// An empty array of all of the birdsongs, which are then manually added thru the editor
	public AudioClip[] listOfSongs;

	// An array of dictionaries of all of the corresponding "correct pitches" for each of the songs.
	// The scripts require songPithces and listOfSongs to have their indexes align.
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


		// Here's where the correct pitches are entered manually.
		// Discrepancy between the pitches detected by the "test" script and the in-game "MicrophoneInput" script.
		// We never found the cause of this. For now, these new values work. Old values kept for reference.

		// birdsong-easy-dingdong
//		songPitches [0].Add (20, 39.0f); old version
//		songPitches [0].Add (23, 21.0f);
		songPitches [0].Add (22, 39.0f);
		songPitches [0].Add (25, 21.0f);

		// birdsong-easy-single
//		songPitches [1].Add (24, 33.0f); old
		songPitches [1].Add (26, 33.0f);

		// birdsong-easy-slide
//		songPitches [2].Add (22, 10.0f);
//		songPitches [2].Add (23, 49.0f);
		songPitches [2].Add (22, 12.0f);
		songPitches [2].Add (23, 51.0f);

		// birdsong-medium-ascending
//		songPitches [3].Add (20, 40.0f);
//		songPitches [3].Add (21, 1.0f);
//		songPitches [3].Add (22, 1.0f);
//		songPitches [3].Add (23, 1.0f);
//		songPitches [3].Add (24, 1.0f);
//		songPitches [3].Add (25, 1.0f);
//		songPitches [3].Add (26, 2.0f);
//		songPitches [3].Add (27, 22.0f);
		songPitches [3].Add (22, 40.0f);
		songPitches [3].Add (23, 1.0f);
		songPitches [3].Add (24, 1.0f);
		songPitches [3].Add (25, 1.0f);
		songPitches [3].Add (26, 1.0f);
		songPitches [3].Add (27, 1.0f);
		songPitches [3].Add (28, 2.0f);
		songPitches [3].Add (29, 22.0f);

		// birdsong-medium-dingdong
//		songPitches [4].Add (19, 21.0f);
//		songPitches [4].Add (23, 28.0f);
//		songPitches [4].Add (26, 23.0f);

		songPitches [4].Add (21, 21.0f);
		songPitches [4].Add (25, 28.0f);
		songPitches [4].Add (28, 23.0f);

		// birdsong-medium-punch
//		songPitches [5].Add (24, 10.0f);
//		songPitches [5].Add (25, 10.0f);
		songPitches [5].Add (26, 10.0f);
		songPitches [5].Add (27, 10.0f);

		// birdsong-hard-ascending
//		songPitches [6].Add (24, 23.0f);
//		songPitches [6].Add (25, 11.0f);
//		songPitches [6].Add (26, 8.0f);
//		songPitches [6].Add (27, 10.0f);
		songPitches [6].Add (26, 23.0f);
		songPitches [6].Add (27, 11.0f);
		songPitches [6].Add (28, 8.0f);
		songPitches [6].Add (29, 10.0f);

		// birdsong-hard-descending
//		songPitches [7].Add (23, 15.0f);
//		songPitches [7].Add (24, 4.0f);
//		songPitches [7].Add (25, 4.0f);
//		songPitches [7].Add (26, 4.0f);
//		songPitches [7].Add (27, 4.0f);
//		songPitches [7].Add (28, 4.0f);
//		songPitches [7].Add (29, 4.0f);
//		songPitches [7].Add (30, 10.0f);
		songPitches [7].Add (25, 15.0f);
		songPitches [7].Add (26, 4.0f);
		songPitches [7].Add (27, 4.0f);
		songPitches [7].Add (28, 4.0f);
		songPitches [7].Add (29, 4.0f);
		songPitches [7].Add (30, 4.0f);
		songPitches [7].Add (31, 4.0f);
		songPitches [7].Add (32, 10.0f);

		// birdsong-hard-slide
//		songPitches [8].Add (21, 9.0f);
//		songPitches [8].Add (22, 1.0f);
//		songPitches [8].Add (23, 1.0f);
//		songPitches [8].Add (24, 1.0f);
//		songPitches [8].Add (26, 24.0f);
//		songPitches [8].Add (27, 18.0f);
//		songPitches [8].Add (28, 8.0f);
		songPitches [8].Add (23, 9.0f);
		songPitches [8].Add (24, 1.0f);
		songPitches [8].Add (25, 1.0f);
		songPitches [8].Add (26, 1.0f);
		songPitches [8].Add (28, 24.0f);
		songPitches [8].Add (29, 18.0f);
		songPitches [8].Add (30, 8.0f);
	}
}
