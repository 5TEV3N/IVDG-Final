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

    [Header("Containers")]
    public GameObject gamePause;
	public GameObject birdCallChecks;
    public GameObject titleScreen;
    public GameObject cameraScreen;
    public GameObject journalIcon;
    public GameObject cameraIcon;

    [Header("Text")]
    public Text birdCallingTutorial;
	public Text discovered;
    public Text loadingBodyText;
    public Text loadingAuthor;
    public Text LoadingSource;

    public Color textColor;

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
    }

    #region Main Menu

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

    public void LoadingScreenTexts(bool gameplayReady)
    {
        if (gameplayReady == true)
        {
            loadingBodyText.gameObject.SetActive(false);
            LoadingSource.gameObject.SetActive(false);
            loadingAuthor.gameObject.SetActive(false);
            LoadGamePlayScene();
        }
        if (gameplayReady == false)
        {
            loadingBodyText.color = Color.Lerp(loadingBodyText.color, Color.white, Time.deltaTime * 5f);
            LoadingSource.color = Color.Lerp(loadingBodyText.color, Color.white, Time.deltaTime * 5f);
            loadingAuthor.color = Color.Lerp(loadingBodyText.color, Color.white, Time.deltaTime * 5f);
        }
    }

    public void LoadGamePlayScene()
    {
        journalIcon.SetActive(true);
        cameraIcon.SetActive(true);
        birdCallChecks.SetActive(true);
        SceneManager.LoadScene("_Gameplay");
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
			TutorialTexts (false,false,false);
		}

    }

	public void TutorialTexts (bool birdTutorial , bool controlsTutorial, bool pictureTutorial)
	{
		if (birdTutorial == true) 
		{
            tutorialBlackBackground.GetComponent<Image>().color = Color.Lerp(tutorialBlackBackground.GetComponent<Image>().color, Color.grey, Time.deltaTime *textSmoothFade);
			birdCallingTutorial.color = Color.Lerp (birdCallingTutorial.color, textColor, Time.deltaTime * textSmoothFade);	
		} 
		else 
		{
			birdCallingTutorial.color = Color.Lerp (birdCallingTutorial.color, Color.clear, Time.deltaTime * textSmoothFade);
            tutorialBlackBackground.GetComponent<Image>().color = Color.Lerp(tutorialBlackBackground.GetComponent<Image>().color, Color.clear, Time.deltaTime * textSmoothFade);
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
    
    #endregion
}