using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionOnLoad : MonoBehaviour
{
    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.localPosition = new Vector3(GameSaveLoad.gameState.data.x, GameSaveLoad.gameState.data.y, GameSaveLoad.gameState.data.z);
            print("Current player position" + player.transform.localPosition);
        }
    }
}
