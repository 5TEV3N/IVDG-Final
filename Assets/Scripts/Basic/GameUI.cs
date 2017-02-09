using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject gamePause;
    public GameObject TitleScreen;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(gamePause != null)
            {
                gamePause.SetActive(false);
            }
            else
            {
                gamePause.SetActive(true);
            }
        }	
	}

    public void NewGame()
    {
        SceneManager.LoadScene("Main");
        print("New Game");
    }

    public void LoadGamePlayScene()
    {
        SceneManager.LoadScene("Main");
        GameSaveLoad.gameState.PlayerLoad();
        print("Loaded Game");
    }

    public void ExitGame()
    {
        GameSaveLoad.gameState.PlayerSave();
        print("Saved the Game!");

        Application.Quit();
    }
}
