using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteEnemySpawner : MonoBehaviour
{
    public GameObject spawnObject;
    public float spawnDelay = 2f;
    GameObject currentObject = null;
    IEnumerator spawnCoroutine;
    bool isSpawning = false;

    void Start()
    {
        isSpawning = false;
        currentObject = null;
    }

    void Update()
    {
        if(currentObject == null && !isSpawning)
        {
            isSpawning = true;
            spawnCoroutine = SpawnObject();
            StartCoroutine(spawnCoroutine);
        }
    }

    IEnumerator SpawnObject()
    {
        yield return new WaitForSeconds(spawnDelay);
        currentObject = Instantiate(spawnObject, transform.position, Quaternion.identity);
        isSpawning = false;
    }
}
