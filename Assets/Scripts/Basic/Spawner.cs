using UnityEngine;
using System.Collections;
[System.Serializable]
public class Spawner
{
	//CREDITS TO Ryon E FOR THE SCRIPT!
    // TO BE REMOVED BECAUSE IT'S NOT BEING USED

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

    public void SpawnObjectAtSpotWithParent(Transform objectSpawnPlace, Transform objectParent , GameObject objectToSpawn)
    {
        GameObject newObjectToSpawn = GameObject.Instantiate(objectToSpawn, objectParent);
        newObjectToSpawn.transform.position = objectSpawnPlace.position;
    }
}
