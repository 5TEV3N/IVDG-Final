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

    //public Vector3 savedPlayerPosition;
    //public Transform savedPlayerPosition;
    //add the lists of screenshots in a gallery here.
}

public class GameSaveLoad : MonoBehaviour
{
    public static GameSaveLoad gameState;
    //[Header("Debug. Please put into private later")]
    //public Transform currentPlayerPosition;
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

    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            //GameObject player = GameObject.FindGameObjectWithTag("Player");
            //player.transform.position = new Vector3(data.x, data.y, data.z);
        }
        //currentPlayerPosition = data.savedPlayerPosition;

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.localPosition = new Vector3(data.x, data.y, data.z);
            print(player.transform.localPosition);
        }
    }

    public void PlayerSave()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        //currentPlayerPosition = data.savedPlayerPosition;
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            data.x = player.transform.localPosition.x;
            data.y = player.transform.localPosition.y;
            data.z = player.transform.localPosition.z;
        }
        //data.savedPlayerPosition = currentPlayerPosition;


        bf.Serialize(file, data);
        file.Close();
    }

    public void PlayerLoad()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            data = (PlayerData)bf.Deserialize(file);
            file.Close();
          

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
//http://answers.unity3d.com/questions/956047/serialize-quaternion-or-vector3.html