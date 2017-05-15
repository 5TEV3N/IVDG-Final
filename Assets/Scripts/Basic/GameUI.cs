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
    BasicFade fade;
    BirdAudioControl birdAudioControler;

    public static GameUI gameUi;
    [Header("DEBUG")]
    public Text birdDisantceUI;
    public Text birdTriggerBirdcallsUI;
    public Text birdTriggerFlyawayUI;
    public Text birdEncounterUI;

    [Header("Containers")]
    public GameObject gamePause;
	public GameObject birdCallChecks;
    public GameObject titleScreen;
    public GameObject cameraFlashPanel;
    public GameObject cameraScreen;
    public GameObject journalIcon;
    public GameObject cameraIcon;

    public GameObject micTutorialIcon;
    public GameObject cameraTutorialIcon;
    public GameObject journalTutorialIcon;

    [Header("Text")]
    public Text birdCallingTutorialText;
    public Text cameraTutorialText;
    public Text journalTutorialText;

    public Text discovered;
    public Text loadingBodyText;    // replace this!
    public Text loadingAuthor;      // replace this!
    public Text LoadingSource;      // replace this!

    [Header("Color")]
    public Color textColor;
    public Color blackTint;

    [Header("Condition Icons")]
    public GameObject[] correctIcons;
    public GameObject[] wrongIcons;
    public GameObject[] successsfulTries;
    public GameObject[] failedTries;

    [Header("Values")]
    public string birdName;
    public float textSmoothFade;
    public float mainMenuToLoadingTimer;
    public float loadingToGameplayTimer;

    [Header("Audio HUD")]
    public GameObject[] audioDots;
    private GameObject audioHUD;

    [Header ("Fade In/Out")]
	public GameObject tutorialBlackBackground;
    public GameObject mainMenuToLoadingBlackBackground;
    public GameObject LoadingToGameplayBlackBackground;

    float mainMenutoLoadingResetTimer;
    float loadingToGameplayResetTimer;

    Scene currentScene;
    string currentSceneName;
    bool readyToPlay;


    void Awake()
    {
        mainMenutoLoadingResetTimer = mainMenuToLoadingTimer;
        loadingToGameplayResetTimer = loadingToGameplayTimer;

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
		journalIcon.SetActive (false);
		cameraIcon.SetActive (false);
		birdCallChecks.SetActive (false);
        
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
        currentScene = SceneManager.GetActiveScene();
        currentSceneName = currentScene.name;
        
        // This section deals with the logic behind the loading screen. WILL BE REPLACED USING COROUTINES
        if (readyToPlay == true)
        {
            FadeIntoLoading();
            mainMenuToLoadingBlackBackground.GetComponent<BasicFade>().startFade = true;
        }
        if (currentSceneName == "LoadingScreen")
        {
            LoadingScreenTexts(false);
            loadingToGameplayTimer = loadingToGameplayResetTimer;
            loadingToGameplayTimer -= Time.time;
            if (loadingToGameplayTimer <= 0f)
            {
                LoadingScreenTexts(true);
            }
        }
        if (currentSceneName == "_Gameplay")
        {
            mainMenuToLoadingBlackBackground.SetActive(false);
            LoadingToGameplayBlackBackground.SetActive(true);
            journalIcon.SetActive(true);
            cameraIcon.SetActive(true);
            birdCallChecks.SetActive(true);
        }
    }

    #region Main Menu
    // This section here deals with the logic behind the main menu. It has some logic for the loading screen but itll be replaced later as seen above
    public void Play()
    {
        UnLoadUI();
        readyToPlay = true;
    }

    public void FadeIntoLoading()
    {
        mainMenuToLoadingTimer = mainMenutoLoadingResetTimer;
        mainMenuToLoadingTimer -= Time.time;
        if (mainMenuToLoadingTimer <= 0)
        {
            SceneManager.LoadScene("LoadingScreen");
            readyToPlay = false;
        }
    }

    public void ExitGame()
    {
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

    #region LoadingScreen

    public void LoadingScreenTexts(bool gameplayReady)         // This section deals with the logic behind the loading screen. WILL BE REPLACED USING COROUTINES
    {
        if (gameplayReady == true)
        {
            loadingBodyText.gameObject.SetActive(false);
            LoadingSource.gameObject.SetActive(false);
            loadingAuthor.gameObject.SetActive(false);
            SceneManager.LoadScene("_Gameplay");
        }
        if (gameplayReady == false)
        {
            loadingBodyText.color = Color.Lerp(loadingBodyText.color, Color.white, Time.deltaTime * 5f);
            LoadingSource.color = Color.Lerp(loadingBodyText.color, Color.white, Time.deltaTime * 5f);
            loadingAuthor.color = Color.Lerp(loadingBodyText.color, Color.white, Time.deltaTime * 5f);
        }
    }
    #endregion

    #region Audio UI
	// This builds the audio HUD (the white/red dots on the right hand side of the screen)
	// All of the dots already exist in the scene, and this script simply hides the notes that are not relevant and shows the notes corresponding to correctNotesArray.
    public void AudioHUDSetup()
    {
        var correctNotesArray = GameObject.FindGameObjectWithTag("Bird").GetComponent<BirdAudioControl>().correctNotesArray;
        audioHUD.transform.Find("Line").GetComponent<Image>().CrossFadeAlpha(1.0f, 0.5f, false);
        for (int i = 0; i < correctNotesArray.Length; i++)
        {
            audioDots[correctNotesArray[i]].transform.Find("white").GetComponent<Image>().CrossFadeAlpha(1.0f, 0.5f, false);
        }
    }

	// This function takes the currentNote being sung (from MicrophoneInput.cs) and fades in the corresponding red dot, to show the player what note they are currently singing.
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
                audioDots[i].transform.Find("red").GetComponent<Image>().CrossFadeAlpha(0.0f, 0.1f, false); // fade out all notes that are not the current note being sung
            }
        }
    }

	// Fade out the entire audio HUD once the bird singing loop is finished
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

    #region Text

    public void BirdDiscovered(bool encountering)               
    {
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

	public void TutorialTexts (bool birdTutorial , bool cameraTutorial, bool journalTutorial)
	{
		if (birdTutorial == true) 
		{
            tutorialBlackBackground.GetComponent<Image>().color = Color.Lerp(tutorialBlackBackground.GetComponent<Image>().color, blackTint, Time.deltaTime *textSmoothFade);
            birdCallingTutorialText.color = Color.Lerp (birdCallingTutorialText.color, textColor, Time.deltaTime * textSmoothFade);
            micTutorialIcon.SetActive(true);
        } 
		else 
		{
            birdCallingTutorialText.color = Color.Lerp (birdCallingTutorialText.color, Color.clear, Time.deltaTime * textSmoothFade);
            tutorialBlackBackground.GetComponent<Image>().color = Color.Lerp(tutorialBlackBackground.GetComponent<Image>().color, Color.clear, Time.deltaTime * textSmoothFade);
            micTutorialIcon.SetActive(false);
        }

        if (cameraTutorial == true)
        {
            cameraTutorialIcon.SetActive(true);
            cameraTutorialText.color = Color.Lerp(cameraTutorialText.color, textColor, Time.deltaTime * textSmoothFade);;
        }
        else
        {
            cameraTutorialIcon.SetActive(false);
            cameraTutorialText.color = Color.Lerp(cameraTutorialText.color, Color.clear, Time.deltaTime * textSmoothFade);
        }

        if (journalTutorial == true)
        {
            journalTutorialText.color = Color.Lerp(journalTutorialText.color, textColor, Time.deltaTime * textSmoothFade);
            journalTutorialIcon.SetActive(true);
        }
        else
        {
            journalTutorialText.color = Color.Lerp(journalTutorialText.color, Color.clear, Time.deltaTime * textSmoothFade);
            journalTutorialIcon.SetActive(false);
        }

	}
    #endregion

    #region General Gameplay UI

    public void InteractionIconsFade(bool fadingIn)
    {
        if (fadingIn == true)
        {
            journalIcon.GetComponent<Image>().CrossFadeAlpha(1f, 0.5f,false);
            cameraIcon.GetComponent<Image>().CrossFadeAlpha(1f, 0.5f, false);
        }
        if (fadingIn == false)
        {
            journalIcon.GetComponent<Image>().CrossFadeAlpha(0.25f, 0.5f, false);
            cameraIcon.GetComponent<Image>().CrossFadeAlpha(0.25f, 0.5f, false);
        }
    }

    public void DisplayBirdcallsIcons(bool displayTheIcons)
    {
        birdAudioControler = GameObject.FindGameObjectWithTag("Bird").GetComponent<BirdAudioControl>();
        int measureTries = birdAudioControler.successNeeded;

        if (displayTheIcons == true)
        {
            for (int i = 0; i < measureTries; i++)
            {
                successsfulTries[i].GetComponentInChildren<Image>().enabled = enabled;
                failedTries[i].GetComponentInChildren<Image>().enabled = enabled;
            }
        }

        if (displayTheIcons == false)
        {
            for (int z = 0; z < measureTries; z++)
            {
                successsfulTries[z].GetComponentInChildren<Image>().enabled = !enabled;
                failedTries[z].GetComponentInChildren<Image>().enabled = !enabled;

                correctIcons[z].GetComponentInChildren<Image>().enabled = false;
                wrongIcons[z].GetComponentInChildren<Image>().enabled = false;
            }
        }
    }

    public void FailedBirdCallIcons(int remainder)
    {
        wrongIcons[remainder].GetComponentInChildren<Image>().enabled = enabled;
    }

    public void SuccessBirdCallIcons(int remainder)
    {
        correctIcons[remainder - 1].GetComponentInChildren<Image>().enabled = enabled;
    }

    public void CameraFlash(bool tookPicture)
    {
        if (tookPicture == true)
        {
            cameraFlashPanel.SetActive(true);
        }
        else
        {
            cameraFlashPanel.GetComponent<Image>().color = new Color(255f, 255f, 255f);
            cameraFlashPanel.SetActive(false);
        }
    }
    #endregion

}