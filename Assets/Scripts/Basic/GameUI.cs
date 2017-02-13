using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public static GameUI gameUi;
    public GameObject gamePause;
    public GameObject TitleScreen;

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

    public void UnLoadUI()
    {
        TitleScreen.SetActive(false);
        gamePause.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

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
}
