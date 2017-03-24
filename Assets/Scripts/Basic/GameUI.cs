using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public static GameUI gameUi;

    public AnimationCurve curveJuice1;

    [Header ("Containers")]
    public GameObject gamePause;
    public GameObject TitleScreen;
    public GameObject micImg;
    private Text discovered;

    [Header("Values")]
    public string birdName;
    public float textSmoothFade;

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

    void Update()
    {
        birdName = BirdSpawner.currentBirdName;     // if we're going for multiple birds in a scene instead of one being moved around at certain distances, then this needs to change;
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

    public void MicInputUI(bool hold)
    {
        if (hold == true)
        {
            micImg.transform.localPosition = Vector2.Lerp(micImg.transform.localPosition, new Vector2(0, -320), curveJuice1.Evaluate(Time.deltaTime * 8));
        }
        else
        {
            micImg.transform.localPosition = Vector2.Lerp(micImg.transform.localPosition, new Vector2(0, -400), curveJuice1.Evaluate(Time.deltaTime * 8));
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