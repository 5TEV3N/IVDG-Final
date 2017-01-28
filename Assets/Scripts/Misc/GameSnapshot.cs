using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSnapshot : MonoBehaviour
{
    //public Image ScreenShot;

    public Text persistentDataPath;
    void Start()
    {
        //Application.CaptureScreenshot("Screenshot " + 1);
        //Application.persistentDataPath;
        persistentDataPath.text = Application.persistentDataPath;
        //Figure out how to retrive these files and be able to present them in-game
        //Lead here?:http://answers.unity3d.com/questions/393431/capturing-screen-shot-and-showing-the-captured-ima.html
    }
}
