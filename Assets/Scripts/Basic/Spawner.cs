using UnityEngine;
using System.Collections;
[System.Serializable]
public class Spawner
{
	//Credits go to Ryon E for this script!

	public Spawner()
    {

    }

	public void SpawnAtStart(Transform[] objectSpawnLocations, GameObject objectToSpawn)
    {
		for (int i = 0; i < objectSpawnLocations.Length; i++)
        { 
			GameObject newObjectToSpawn = GameObject.Instantiate (objectToSpawn);
			newObjectToSpawn.transform.position = objectSpawnLocations [i].position;
		}
	}

    public void SpawnObjectAtRandomLocation(int objectsToSpawnAtRandom, Transform[] objectSpawnLocations , GameObject objectToSpawn)
    {
		for (int i = 0; i < objectsToSpawnAtRandom; i++)
        { 
			GameObject newObjectToSpawn = GameObject.Instantiate (objectToSpawn);
			newObjectToSpawn.transform.position = objectSpawnLocations [Random.Range (0, objectSpawnLocations.Length)].position;
		}
	}

    public void SpawnObjectAtSpot(Transform objectSpawnPlace, GameObject objectToSpawn)
    {
		GameObject newObjectToSpawn = GameObject.Instantiate (objectToSpawn);
		newObjectToSpawn.transform.position = objectSpawnPlace.position;
	}
}
