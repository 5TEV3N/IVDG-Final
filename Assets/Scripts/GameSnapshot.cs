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
    }
}
