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
    public static GameUI gameUi;

    public AnimationCurve curveJuice1;

    [Header ("Containers")]
    public GameObject gamePause;
    public GameObject TitleScreen;
//    public GameObject micImg;
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

	void Start() {
		audioDots = new GameObject[37]; // 37 is the number of notes measured by our audio script
		audioHUD = GameObject.Find("AudioHUD");

		for (int i = 0; i < 37; i++) {
			var dotName = i.ToString ();
			var allDots = GameObject.FindGameObjectsWithTag ("AudioDot");
			foreach (GameObject dot in allDots) {
				if (dot.name == dotName) {
					audioDots [i] = dot;
					audioDots [i].transform.Find ("red").GetComponent<Image> ().CrossFadeAlpha (0.0f, 0.01f, false); // hide Red on default
					audioDots [i].transform.Find ("white").GetComponent<Image> ().CrossFadeAlpha (0.0f, 0.01f, false); // hide Red on default
				}
			}
		}

		audioHUD.transform.Find ("Line").GetComponent<Image> ().CrossFadeAlpha (0.0f, 0.01f, false);
	}

    void Update()
    {
        birdName = BirdSpawner.currentBirdName;     // if we're going for multiple birds in a scene instead of one being moved around at certain distances, then this needs to change;

//		if (Input.GetKeyUp (KeyCode.Alpha9) {
//			
//		}
    }

    #region Main Menu

    public void NewGame()
    {
        UnLoadUI();
        SceneManager.LoadScene("Main");
        print("New Game");
    }

    public void LoadGamePlayScene()
    {
        UnLoadUI();
        SceneManager.LoadScene("Main");
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
        TitleScreen.SetActive(false);
        gamePause.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }
    
    #endregion

//    public void MicInputUI(bool mic)
//    {
//        if (mic == true)
//        {
//            micImg.transform.localPosition = Vector2.Lerp(micImg.transform.localPosition, new Vector2(0, -320), curveJuice1.Evaluate(Time.deltaTime * 8));
//        }
//        else
//        {
//            micImg.transform.localPosition = Vector2.Lerp(micImg.transform.localPosition, new Vector2(0, -400), curveJuice1.Evaluate(Time.deltaTime * 8));
//        }
//    }


	// AUDIO UI FROM MICROPHONE INPUT
	public void AudioHUDSetup() {
		var correctNotesArray = GameObject.FindGameObjectWithTag("Bird").GetComponent<BirdAudioControl>().correctNotesArray;

		audioHUD.transform.Find ("Line").GetComponent<Image> ().CrossFadeAlpha (1.0f, 0.5f, false);
		for (int i = 0; i < correctNotesArray.Length; i++) {
			audioDots [correctNotesArray[i]].transform.Find ("white").GetComponent<Image> ().CrossFadeAlpha (1.0f, 0.5f, false);
		}
	}

	public void AudioHUDCurrentNote(int currentNote) {
		for (int i = 0; i < audioDots.Length; i++) {
			if (i == currentNote) {
				audioDots [i].transform.Find ("red").GetComponent<Image> ().CrossFadeAlpha (1.0f, 0.1f, false);
			} else {
				audioDots [i].transform.Find ("red").GetComponent<Image> ().CrossFadeAlpha (0.0f, 0.1f, false);
			}
		}
	}

	public void AudioHUDClear() {
		audioHUD.transform.Find ("Line").GetComponent<Image> ().CrossFadeAlpha (0.0f, 1.2f, false);
		for (int i = 0; i < audioDots.Length; i++) {
			audioDots [i].transform.Find ("red").GetComponent<Image> ().CrossFadeAlpha (0.0f, 1.0f, false);
			audioDots [i].transform.Find ("white").GetComponent<Image> ().CrossFadeAlpha (0.0f, 1.0f, false);
		}
	}

    public void BirdDiscovered(bool encountering)
    {
        discovered = GameObject.FindGameObjectWithTag("UIBirdName").GetComponent<Text>();
        if (encountering == true)
        {
            discovered.text = "Discovered a\n " + birdName;
            discovered.color = Color.Lerp(discovered.color, Color.black, Time.deltaTime * textSmoothFade);
        }
        else
        {
            discovered.color = Color.Lerp(discovered.color, Color.clear, Time.deltaTime * textSmoothFade); 
        }
    }

}