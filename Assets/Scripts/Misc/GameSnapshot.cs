using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameSnapshot : MonoBehaviour
{
    public Texture2D screenShot;
    public Text persistentDataPath;
    private string screenshotName;
    private bool check;

    void Start()
    {
        persistentDataPath.text = Application.persistentDataPath;
        screenShot = new Texture2D(300, 200, TextureFormat.RGB24, false); // 1
    }

    private void OnGUI()
    {
        if (check == true)
        {
            GUI.DrawTexture(new Rect(10, 10, 60, 40), screenShot, ScaleMode.StretchToFill);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.End))
        {
            Application.CaptureScreenshot(screenshotName);
            StartCoroutine("GetSnapshot");
        }
    }

    IEnumerator GetSnapshot()
    {
        yield return new WaitForEndOfFrame();
        screenShot.ReadPixels(new Rect(0, 0, 1920, 1080), 0, 0);
        screenShot.Apply();
        
        byte[] bytes = screenShot.EncodeToPNG();
        screenshotName = "/screenshot" + Time.frameCount + ".png";
        File.WriteAllBytes(Application.dataPath + screenshotName, bytes);
        check = true;
    }
}
//Figure out how to retrive these files and be able to present them in-game
//Lead here?:http://answers.unity3d.com/questions/393431/capturing-screen-shot-and-showing-the-captured-ima.html
//https://aarlangdi.blogspot.ca/2016/07/saving-screen-shot-in-unity-3d.html