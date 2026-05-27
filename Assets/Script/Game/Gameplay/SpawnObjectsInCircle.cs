using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectsInCircle : MonoBehaviour
{
    [SerializeField]
    private int numberOfObjects = 10;
    [SerializeField]
    float radius = 5f;
    [SerializeField]
    private GameObject spawnObject;

    public List<GameObject> spawnedObjects = new List<GameObject>();
    private void Start()
    {
        SpawnObjects();
        ActivateSpawnedObjects();
    }
    public void SpawnObjects()
    {
        for(int i = 0; i < numberOfObjects; i++)
        {
            int numberOfTurns = 360 / numberOfObjects;
            transform.rotation *= Quaternion.Euler(0, 0, numberOfTurns);
            Vector3 spawnPosition = Vector3.up * radius;
            Vector3 randomOffset = Vector3.Lerp(transform.position, spawnPosition, Random.Range(.15f,1f));
            GameObject newObj = Instantiate(spawnObject, randomOffset, Quaternion.identity, transform);
            spawnedObjects.Add(newObj);
            newObj.SetActive(false);
        }
    }
    public void ActivateSpawnedObjects()
    {
        spawnedObjects[0].SetActive(true);
    }

    public void DestroyOnHit()
    {
        GameObject objToDestroy = spawnedObjects[0];

        spawnedObjects.RemoveAt(0);

        Destroy(objToDestroy);

        if (spawnedObjects.Count > 0)
        {
            spawnedObjects[0].SetActive(true);
        }
        else 
        { 
            FindFirstObjectByType<GoToScene>().GoToSceneEvent();
        }
        
    }
}
