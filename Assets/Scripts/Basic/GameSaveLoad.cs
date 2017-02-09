using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
class PlayerData
{
    public Vector3 savedPlayerPosition;
    //add the lists of screenshots in a gallery here.
}

public class GameSaveLoad : MonoBehaviour
{
    public static GameSaveLoad gameState;
    public Vector3 currentPlayerPosition;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            currentPlayerPosition = player.transform.position;
        }
    }

    public void PlayerSave()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData data = new PlayerData();
        data.savedPlayerPosition = currentPlayerPosition;

        bf.Serialize(file, data);
        file.Close();
    }

    public void PlayerLoad()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            currentPlayerPosition = data.savedPlayerPosition;
        }
    }

    public void ScreenshotSave()
    {

    }

    public void ScreenshotLoad()
    {

    }
}
//REFFERENCE
//https://unity3d.com/learn/tutorials/topics/scripting/persistence-saving-and-loading-data