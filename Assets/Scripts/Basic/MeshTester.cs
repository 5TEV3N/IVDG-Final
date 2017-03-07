using UnityEngine;
using System.Collections.Generic;

public class MeshTester : MonoBehaviour
{
    // CREDITS TO masterprompt FOR THE SCRIPT
    public GameObject avatar;
    public GameObject wornClothing;
    [Space(20)]
    public List<GameObject> clothing;
	const int buttonWidth = 200;
	const int buttonHeight = 20;
	private MeshStitcher stitcher;

	public void Awake ()
	{
		stitcher = new MeshStitcher();
	}

	public void OnGUI ()
	{
		var offset = 0;
		foreach (var cloth in clothing)
        {
			if (GUI.Button (new Rect (0, offset, buttonWidth, buttonHeight), cloth.name))
            {
				RemoveWorn ();
				Wear (cloth);
			}
			offset += buttonHeight;
		}
	}


	private void RemoveWorn ()
	{
        if (wornClothing == null)
        {
            print("Nothing to remove");
            return;
        }
		GameObject.Destroy (wornClothing);
	}

	private void Wear (GameObject clothing)
	{
        if (clothing == null)
        {
            print("Currently wearing something");
            return;
        }
		clothing = GameObject.Instantiate (clothing);
		wornClothing = stitcher.Stitch (clothing, avatar);
		GameObject.Destroy (clothing);
	}
}
