using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(GameScreenshot))]
[RequireComponent(typeof(ScreenshotToJournal))]
[RequireComponent(typeof(BirdInfoToJournal))]

public class GameUI : MonoBehaviour
{
    BirdState myState;

    public static GameUI gameUi;

    [Header("Containers")]
    public GameObject gamePause;
    public GameObject titleScreen;
    public GameObject cameraScreen;
    public GameObject journalIcon;
    public GameObject cameraIcon;

    public Color textColor;

    private Text discovered;

    [Header("Values")]
    public string birdName;
    public float textSmoothFade;

    [Header("Audio HUD")]
    public GameObject[] audioDots;
    private GameObject audioHUD;

    void Awake()
    {
        if (gameUi == null)
        {
            DontDestroyOnLoad(gameObject);
            gameUi = this;
        }
        else if (gameUi != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioDots = new GameObject[37]; // 37 is the number of notes measured by our audio script
        audioHUD = GameObject.Find("AudioHUD");

        for (int i = 0; i < 37; i++)
        {
            var dotName = i.ToString();
            var allDots = GameObject.FindGameObjectsWithTag("AudioDot");
            foreach (GameObject dot in allDots)
            {
                if (dot.name == dotName)
                {
                    audioDots[i] = dot;
                    audioDots[i].transform.Find("red").GetComponent<Image>().CrossFadeAlpha(0.0f, 0.01f, false); // hide Red on default
                    audioDots[i].transform.Find("white").GetComponent<Image>().CrossFadeAlpha(0.0f, 0.01f, false); // hide Red on default
                }
            }
        }

        audioHUD.transform.Find("Line").GetComponent<Image>().CrossFadeAlpha(0.0f, 0.01f, false);
    }

    void Update()
    {
        birdName = BirdSpawner.currentBirdName;
    }

    #region Main Menu

    public void NewGame()
    {
        UnLoadUI();
        SceneManager.LoadScene("Main");
        myState = GameObject.FindGameObjectWithTag("Bird").GetComponent<BirdState>();
        print("New Game");
    }

    public void LoadGamePlayScene()
    {
        UnLoadUI();
        SceneManager.LoadScene("LevelWhiteBox");
        GameSaveLoad.gameState.PlayerLoad();
    }

    public void MainMenuExitGame()
    {
        Application.Quit();
    }

    public void ExitGame()
    {
        UnLoadUI();
        GameSaveLoad.gameState.PlayerSave();

        Application.Quit();
    }

    public void UnLoadUI()
    {
        titleScreen.SetActive(false);
        gamePause.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    #endregion

    #region Audio UI
    // AUDIO UI FROM MICROPHONE INPUT
    public void AudioHUDSetup()
    {
        var correctNotesArray = GameObject.FindGameObjectWithTag("Bird").GetComponent<BirdAudioControl>().correctNotesArray;

        audioHUD.transform.Find("Line").GetComponent<Image>().CrossFadeAlpha(1.0f, 0.5f, false);
        for (int i = 0; i < correctNotesArray.Length; i++)
        {
            audioDots[correctNotesArray[i]].transform.Find("white").GetComponent<Image>().CrossFadeAlpha(1.0f, 0.5f, false);
        }
    }

    public void AudioHUDCurrentNote(int currentNote)
    {
        for (int i = 0; i < audioDots.Length; i++)
        {
            if (i == currentNote)
            {
                audioDots[i].transform.Find("red").GetComponent<Image>().CrossFadeAlpha(1.0f, 0.1f, false);
            }
            else
            {
                audioDots[i].transform.Find("red").GetComponent<Image>().CrossFadeAlpha(0.0f, 0.1f, false);
            }
        }
    }

    public void AudioHUDClear()
    {
        audioHUD.transform.Find("Line").GetComponent<Image>().CrossFadeAlpha(0.0f, 1.2f, false);
        for (int i = 0; i < audioDots.Length; i++)
        {
            audioDots[i].transform.Find("red").GetComponent<Image>().CrossFadeAlpha(0.0f, 1.0f, false);
            audioDots[i].transform.Find("white").GetComponent<Image>().CrossFadeAlpha(0.0f, 1.0f, false);
        }
    }
    #endregion

    #region Bird Related
    public void BirdDiscovered(bool encountering)
    {
        discovered = GameObject.FindGameObjectWithTag("UIBirdName").GetComponent<Text>();
        if (encountering == true)
        {
            discovered.text = "Discovered a\n " + birdName;
            discovered.color = Color.Lerp(discovered.color, textColor, Time.deltaTime * textSmoothFade);
        }
        else
        {
            discovered.color = Color.Lerp(discovered.color, Color.clear, Time.deltaTime * textSmoothFade);
        }
    }
    #endregion

    #region General Gameplay UI
    public void IconFadeIn(bool fadingIn)
    {
        if (fadingIn == true)
        {
            journalIcon.GetComponent<Image>().CrossFadeAlpha(1f, 1f,false);
            cameraIcon.GetComponent<Image>().CrossFadeAlpha(1f, 1f, false);
        }
    }
    #endregion
}