using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class PlayerData
{
    public float x;
    public float y;
    public float z;
}

public class GameSaveLoad : MonoBehaviour
{
    public static GameSaveLoad gameState;
    public PlayerData data = new PlayerData();

    void Awake()
    {
        if (gameState == null)
        {
            DontDestroyOnLoad(gameObject);
            gameState = this;
        }
        else if (gameState != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlayerSave()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            data.x = player.transform.localPosition.x;
            data.y = player.transform.localPosition.y;
            data.z = player.transform.localPosition.z;
        }

        print(data.x + "x" + data.y + "y" + data.z + "z" + "  Player position has been saved!");

        bf.Serialize(file, data);
        file.Close();
    }

    public void PlayerLoad()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            data = (PlayerData)bf.Deserialize(file);
            print(data.x + "x" + data.y + "y" + data.z + "z" + "  Player position has been loaded!");

            file.Close();
        }

        else { print("Error: Nothing was saved..."); }
    }
}
//REFFERENCE
//https://unity3d.com/learn/tutorials/topics/scripting/persistence-saving-and-loading-data
//http://answers.unity3d.com/questions/956047/serialize-quaternion-or-vector3.html