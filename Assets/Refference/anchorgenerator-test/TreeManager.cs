using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TreeManager : MonoBehaviour
{
    public GameObject tree;
    public Transform[] treeAnchors;
    public Spawner spawn = new Spawner();

    void Awake()
    {
        spawn.SpawnObjectAtRandomLocation(treeAnchors.Length, treeAnchors, tree);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SpawnTesting");
        }
    }
}
