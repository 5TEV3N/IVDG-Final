using UnityEngine;
using System.Collections;

public class visualresponse : MonoBehaviour {

	public int multiplier = 5;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		var currentVolume = GameObject.Find("Microphone").GetComponent<mictest>().testSound;
		Vector3 currentScale = new Vector3 (1, currentVolume * multiplier, 1);
		gameObject.transform.localScale = currentScale;
	}
}
